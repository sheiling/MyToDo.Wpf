using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels.Dialogs
{
    public class MsgViewModel : BindableBase,IDialogHostAware
    {
        private string title;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }

        private string content;

        public string Content
        {
            get => content;
            set
            {
                content = value;
                RaisePropertyChanged();
            }
        }

        public string DialogHostName { get; set; } = "Root";
        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Title"))
                Title = parameters.GetValue<string>("Title");
            if (parameters.ContainsKey("Content"))
                Content = parameters.GetValue<string>("Content");

            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void OnCancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel));
        }

        private void OnSave()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                var parameter = new DialogParameters();
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameter));
            }
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
    }
}
