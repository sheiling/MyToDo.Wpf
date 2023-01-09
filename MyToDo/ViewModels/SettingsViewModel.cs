using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Common;
using Prism.Mvvm;

namespace MyToDo.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private ObservableCollection<MenuBar> menuSettings;
        public ObservableCollection<MenuBar> MenuSettings
        {
            get { return menuSettings; }
            set { menuSettings = value; RaisePropertyChanged(); }
        }

        public SettingsViewModel()
        {
            menuSettings = new ObservableCollection<MenuBar>();
            CreateMenus();
        }

        private void CreateMenus()
        {
            menuSettings.Add(new MenuBar()
            {
                Icon = "PaletteOutline",
                Name = "个性化",
                NameSpace = "IndexView"
            });
            menuSettings.Add(new MenuBar()
            {
                Icon = "CogOutline",
                Name = "系统设置",
                NameSpace = "ToDoView"
            });
            menuSettings.Add(new MenuBar()
            {
                Icon = "InformationOutline",
                Name = "关于更多",
                NameSpace = "MemoView"
            });
        }
    }
}
