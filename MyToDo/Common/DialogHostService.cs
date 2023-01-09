using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;


namespace MyToDo.Common
{
    public class DialogHostService : DialogService,IDialogHostService
    {
        private readonly IContainerExtension _containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            _containerExtension = containerExtension;
        }

        public async Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root")
        {
            if (parameters == null)
                parameters = new DialogParameters();

            var content = _containerExtension.Resolve<object>(name);
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a framework!");

            if (dialogContent is FrameworkElement view && view.DataContext is null &&
                ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view,true);

            if (!(dialogContent.DataContext is IDialogHostAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement!");

            viewModel.DialogHostName = dialogHostName;
            DialogOpenedEventHandler eventHandler = (sender, eventArgs) =>
            {
                if(viewModel is IDialogHostAware aware)
                    aware.OnDialogOpend(parameters);
                eventArgs.Session.UpdateContent(content);
            };

            return (IDialogResult) await DialogHost.Show(dialogContent, viewModel.DialogHostName,
                eventHandler);
        }
    }
}
