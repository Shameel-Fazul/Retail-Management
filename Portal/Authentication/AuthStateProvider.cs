using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace Portal.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _config;
        private readonly IAPIHelper _apiHelper;
        private readonly ILoggedInUserModel _loggedInUser;
        private readonly AuthenticationState _anonymous; 

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage, IConfiguration config, IAPIHelper apiHelper, ILoggedInUserModel loggedInUser)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _config = config;
            _apiHelper = apiHelper;
            _loggedInUser = loggedInUser;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authTokenStorageKey = _config["authTokenStorageKey"];
            var token = await _localStorage.GetItemAsync<string>(authTokenStorageKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous;
            }

            bool isAuthenticated = await NotifyUserAuthentication(token);

            if (isAuthenticated == false)
            {
                return _anonymous;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
        }

        public async Task<bool> NotifyUserAuthentication(string token)
        {
            Task<AuthenticationState> authState;
            bool isAutheticatedCheck;

            try
            {
                await _apiHelper.GetLoggedInUserInfo(token);
                Console.WriteLine($"{_loggedInUser.FirstName} just logged in!");
                var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
                authState = Task.FromResult(new AuthenticationState(authenticatedUser));
                NotifyAuthenticationStateChanged(authState);
                isAutheticatedCheck = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await NotifyUserLogout();
                isAutheticatedCheck = false;
            }

            return isAutheticatedCheck;
        }

        public async Task NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);

            string authTokenStorageKey = _config["authTokenStorageKey"];
            await _localStorage.RemoveItemAsync(authTokenStorageKey);
            
            _loggedInUser.ResetUserModel();
            _apiHelper.LogOffUser();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            NotifyAuthenticationStateChanged(authState);
        }
    }
}
