using MyToDo.Common;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

namespace MyToDo.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        private readonly IRegionManager _regionManager;
        private readonly IContainerProvider _provider;
        private ObservableCollection<MenuBar> menuBars;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        private string userName;
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                RaisePropertyChanged();
            }
        }


        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }

        public DelegateCommand LoginOutCommand { get; private set; }

        public MainViewModel(IRegionManager regionManager,IContainerProvider provider)
        {
            _regionManager = regionManager;
            _provider = provider;
            menuBars = new ObservableCollection<MenuBar>();

            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            LoginOutCommand = new DelegateCommand(OnLoginOut);
        }

        private void OnLoginOut()
        {
            App.LoginOut(_provider);
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.NameSpace))
                return;

            _regionManager.Regions[PrismManager.PrismMainRegionName].RequestNavigate(obj.NameSpace);
        }

        private void CreateMenus()
        {
            menuBars.Add(new MenuBar()
            {
                Icon = "Home",
                Name = "首页",
                NameSpace = "IndexView"
            });
            menuBars.Add(new MenuBar()
            {
                Icon = "NotebookOutline",
                Name = "待办事项",
                NameSpace = "ToDoView"
            });
            menuBars.Add(new MenuBar()
            {
                Icon = "NotebookPlus",
                Name = "备忘录",
                NameSpace = "MemoView"
            });
            menuBars.Add(new MenuBar()
            {
                Icon = "Cog",
                Name = "设置",
                NameSpace = "SettingsView"
            });
        }

        public void Configure()
        {
            UserName = PrismManager.AppSession;
            CreateMenus();
            _regionManager.Regions[PrismManager.PrismMainRegionName].RequestNavigate("IndexView");
        }
    }
}
