using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDo.ViewModels.Dialogs
{
    public class AddToDoViewModel: BindableBase, IDialogHostAware
    {
        public string DialogHostName { get; set; }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
                Model = parameters.GetValue<ToDoDto>("Value");
            else
                Model = new ToDoDto();
            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private ToDoDto model;
        public ToDoDto Model
        {
            get => model;
            set
            {
                model = value;
                RaisePropertyChanged();
            }
        }

        private void OnCancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel));
        }

        private void OnSave()
        {
            if (string.IsNullOrEmpty(Model.Title) || string.IsNullOrEmpty(Model.Content))
                return;

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                var parameter = new DialogParameters();
                parameter.Add("Value",Model);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameter));
            }
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
    }
}
