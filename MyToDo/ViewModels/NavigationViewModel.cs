using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Events;
using MyToDo.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace MyToDo.ViewModels
{
    public class NavigationViewModel : BindableBase, INavigationAware
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            _eventAggregator = _containerProvider.Resolve<IEventAggregator>();
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new Exception();
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new Exception();
        }

        public virtual void UpdateLoading(bool isOpen)
        {
            _eventAggregator.UpdateLoading(new UpdateModel()
            {
                IsOpenModel = isOpen
            });
        }
    }
}
