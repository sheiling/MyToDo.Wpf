using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace MyToDo.Common
{
    public interface IDialogHostAware 
    {
        string DialogHostName { get; set; }
        void OnDialogOpend(IDialogParameters parameters);
        DelegateCommand SaveCommand { get; set; }
        DelegateCommand CancelCommand { get; set; }
    }
}
