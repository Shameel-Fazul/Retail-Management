using RMDeskopUI.Models;
using System.Threading.Tasks;

namespace RMDeskopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}