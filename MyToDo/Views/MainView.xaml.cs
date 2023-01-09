using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyToDo.Common;
using MyToDo.Extensions;
using Prism.Events;
using Prism.Services.Dialogs;

namespace MyToDo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IEventAggregator aggregator,IDialogHostService dialogHost)
        {
            InitializeComponent();

            aggregator.Register(arg =>
            {
                DialogHost.IsOpen = arg.IsOpenModel;
                if (DialogHost.IsOpen)
                    DialogHost.DialogContent = new ProgressView();
            });

            btnMin.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };

            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };

            btnClose.Click += async (s, e) =>
            {
                var result = await dialogHost.Question("温馨提示", "是否退出？");
                if (result.Result != ButtonResult.OK)
                    return;
                this.Close();
            };

            colorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };

            colorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

            listBox.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
        }
    }
}
