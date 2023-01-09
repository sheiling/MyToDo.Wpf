using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Events;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;


namespace MyToDo.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly IEventAggregator _aggregator;

        // 设置服务对象 
        private readonly LoginService _loginService;

        private UserDto uDto;
        public UserDto UDto
        {
            get => uDto;
            set
            {
                uDto = value;
                RaisePropertyChanged();
            }
        }

        private int selectIndex;
        public int SelectIndex
        {
            get => selectIndex; 
            set
            {
                selectIndex = value;
                RaisePropertyChanged();
            }
        }

        private string twicePassword;
        public string TwicePassword
        {
            get => twicePassword;
            set
            {
                twicePassword = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public LoginViewModel(IContainerProvider provider,IEventAggregator aggregator)
        {
            _aggregator = aggregator;
            SelectIndex = 0;
            uDto = new UserDto();
            ExecuteCommand = new DelegateCommand<string>(OnExecute);

            _loginService = provider.Resolve<LoginService>();
        }

        private void OnExecute(string obj)
        {
            switch (obj)
            {
                case "Login":
                    Login();
                    break;
                case "Register":
                    Register();
                    break;
                case "RegisterPage":
                    SelectIndex = 1;
                    break;
                case "Return":
                    SelectIndex = 0;
                    break;
            }
        }

        public async void Register()
        {
            if (string.IsNullOrWhiteSpace(UDto.Name) || string.IsNullOrWhiteSpace(UDto.Password)
                || string.IsNullOrWhiteSpace(uDto.Account)
                || string.IsNullOrWhiteSpace(TwicePassword))
            {
                _aggregator.SendMessage("内容不能为空", "Login");
                return;
            }

            if (TwicePassword != UDto.Password)
            {
                _aggregator.SendMessage("两次密码不一致", "Login");
                return;
            }

            var result = await _loginService.RegisterAsync(UDto);
            if (result.Status)
            {
                _aggregator.SendMessage("注册成功", "Login");
                SelectIndex = 0;
                return;
            }
            _aggregator.SendMessage($"{UDto.Name}帐号注册失败", "Login");
        }

        public async void Login()
        {
            if (string.IsNullOrWhiteSpace(UDto.Name) || string.IsNullOrWhiteSpace(UDto.Password))
            {
                _aggregator.SendMessage("用户名和密码不能为空", "Login");
                return;
            }

            var result = await _loginService.LoginAsync(new UserDto()
            {
                Name = UDto.Name,
                Password = UDto.Password
            });

            if (result.Status)
            {
                PrismManager.AppSession = result.Result.Account;
                _aggregator.SendMessage("登录成功", "Login");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                return;
            }
            _aggregator.SendMessage(result.Message, "Login");
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public string Title { get; set; } = "ToDo";
        public event Action<IDialogResult> RequestClose;
    }
}
