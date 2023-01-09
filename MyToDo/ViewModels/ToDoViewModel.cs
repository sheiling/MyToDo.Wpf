using System;
using System.Collections.ObjectModel;
using System.Linq;
using ImTools;
using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;


namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        private readonly IToDoService _service;
        private ObservableCollection<ToDoDto> todoList;
        public ObservableCollection<ToDoDto> ToDoList
        {
            get => todoList;
            set
            {
                todoList = value;
                RaisePropertyChanged();
            }
        }

        private bool _isRightOpen;
        public bool IsRightOpen
        {
            get => _isRightOpen;
            set
            {
                _isRightOpen = value;
                RaisePropertyChanged();
            }
        }

        private ToDoDto currentDto;
        public ToDoDto CurrentDto
        {
            get => currentDto;
            set
            {
                currentDto = value;
                RaisePropertyChanged();
            }
        }

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                _search = value;
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

        public DelegateCommand<string> SearchCommand { get; private set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }


        public ToDoViewModel(IToDoService service, IContainerProvider provider) : base(provider)
        {
            _service = service;
            todoList = new ObservableCollection<ToDoDto>();
            SelectedCommand = new DelegateCommand<ToDoDto>(OnSelectedCommand);
            ExecuteCommand = new DelegateCommand<string>(OnExecute);
            SearchCommand = new DelegateCommand<string>(OnSearch);

            dialogHost = provider.Resolve<IDialogHostService>();
            DeleteCommand = new DelegateCommand<ToDoDto>(OnDelete);
        }

        private async void OnDelete(ToDoDto obj)
        {
            try
            {
                var resultDialog = await dialogHost.Question("温馨提示", $"是否删除待办事项:{obj.Title} ？");
                if (resultDialog.Result != ButtonResult.OK)
                    return;

                UpdateLoading(true);
                var result = await _service.DeleteAsync(obj.Id);
                if (result.Status)
                {
                    var to = todoList.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (to != null)
                        ToDoList.Remove(to);
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private  void OnSearch(string obj)
        {
            GetDataAsync(obj);
        }

        private void OnExecute(string obj)
        {
            switch (obj)
            {
                case "新增":
                    Add();
                    break;
                case "保存":
                    Save();
                    break;
                default:
                    break;
            }
        }

        private void Add()
        {
            CurrentDto = new ToDoDto();
            IsRightOpen = true;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(CurrentDto.Content) ||
                string.IsNullOrEmpty(CurrentDto.Title))
                return;

            try
            {
                UpdateLoading(true);
                if (CurrentDto.Id > 0)
                { 
                    // 更新数据
                   var result = await _service.UpdateAsync(CurrentDto);
                   if (result.Status)
                   {
                       var toDo = ToDoList.FindFirst(t => t.Id == currentDto.Id);
                       if (toDo != null)
                       {
                           toDo.Title = CurrentDto.Title;
                           toDo.Content = CurrentDto.Content;
                           toDo.Status = CurrentDto.Status;
                       }
                   }
                   IsRightOpen = false;
                }
                else
                {
                    // 添加数据
                    var result = await _service.AddAsync(CurrentDto);
                    if (result.Status)
                        ToDoList.Add(CurrentDto);

                    IsRightOpen = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void OnSelectedCommand(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var result = await _service.GetFirstOfDefaultAsync(obj.Id);
                if (result.Status)
                {
                    IsRightOpen = true;
                    CurrentDto = result.Result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (navigationContext.Parameters.ContainsKey("Value"))
                SelectIndex = navigationContext.Parameters.GetValue<int>("Value");
            else
                SelectIndex = 0;

            GetDataAsync();
        }


        private async void GetDataAsync(string search = "")
        {
            try
            {
                UpdateLoading(true);
                int? status = SelectIndex == 0 ? null : SelectIndex == 1 ? 0 : 1;

                var result = await _service.GetAllAsync(new ToDoParameter()
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = search,
                    Status = status
                });

                todoList.Clear();
                if (result != null && result.Status)
                {
                    foreach (var item in result.Result.Items)
                    {
                        todoList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                UpdateLoading(false);
            }
        }
    }
}