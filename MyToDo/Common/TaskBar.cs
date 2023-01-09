using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace MyToDo.Common
{
    public class TaskBar : BindableBase
    {
        private string content;
        public string Icon { get; set; }

        public string Content
        {
            get => content;
            set
            {
                content = value;
                RaisePropertyChanged();
            }
        }

        public string Title { get; set; }
        public string Color { get; set; }
        public string Target { get; set; }
    }
}
