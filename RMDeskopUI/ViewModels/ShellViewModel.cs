using Caliburn.Micro;
using RMDeskopUI.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDeskopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private LoginViewModel _loginVM;
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private SimpleContainer _container;

        public ShellViewModel(LoginViewModel loginVM, IEventAggregator events, SalesViewModel salesVM, SimpleContainer container)
        {
            _events = events;
            _loginVM = loginVM;
            _salesVM = salesVM;
            _container = container;

            _events.Subscribe(this);
            
            ActivateItem(_loginVM);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
            _loginVM = _container.GetInstance<LoginViewModel>(); // overwrite current loginvm instance with new instance to wipe all existing data
        }
    }
}
