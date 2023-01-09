using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using DryIoc;
using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;


namespace MyToDo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IRegionManager _regionManager;

        private readonly IMemoService memoService;
        private readonly IToDoService toDoService;

        private readonly IDialogHostService _service;
        private ObservableCollection<TaskBar> taskBars;
        public ObservableCollection<TaskBar> TaskBars
        {
            get => taskBars;
            set
            {
                taskBars = value;
                RaisePropertyChanged();
            }
        }

        private SummaryDto summDto;

        public SummaryDto SummDto
        {
            get => summDto;
            set
            {
                summDto = value;
                RaisePropertyChanged();
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<string> AddCommand { get; private set; }

        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }

        public DelegateCommand<ToDoDto> CompletedCommand { get; private set; }
        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }

        public IndexViewModel(IDialogHostService service, IContainerProvider provider) : base(provider)
        {
            Title = $"你好，{PrismManager.AppSession}！今天是{DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";
            _service = service;
            CreateTaskBar();

            AddCommand = new DelegateCommand<string>(OnAdd);

            memoService = provider.Resolve<IMemoService>();
            toDoService = provider.Resolve<IToDoService>();
            _regionManager = provider.Resolve<IRegionManager>();

            SummDto = new SummaryDto();
            GetDataAsync();

            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            CompletedCommand = new DelegateCommand<ToDoDto>(OnCompleted);

            NavigateCommand = new DelegateCommand<TaskBar>(OnNavigate);
        }

        private void OnNavigate(TaskBar obj)
        {
            if (string.IsNullOrEmpty(obj.Target))
                return;

            NavigationParameters parameters = new NavigationParameters();
            if (obj.Title == "汇总")
                parameters.Add("Value", 1);
            else if (obj.Title == "已完成")
                parameters.Add("Value", 2);

            _regionManager.Regions[PrismManager.PrismMainRegionName].RequestNavigate(obj.Target, parameters);
        }

        private async void OnCompleted(ToDoDto obj)
        {
            var result = await toDoService.UpdateAsync(obj);
            if (result.Status)
            {
                var model = SummDto.ToDoList.FirstOrDefault(t => t.Id.Equals(obj.Id));
                if (model != null)
                    SummDto.ToDoList.Remove(model);
            }
        }

        private void OnAdd(string obj)
        {
            switch (obj)
            {
                case "添加待办":
                    AddToDo(null);
                    break;
                case "添加备忘录":
                    AddMemo(null);
                    break;
            }
        }

        private async void AddToDo(ToDoDto model)
        {
            var param = new DialogParameters();
            if (model != null)
                param.Add("Value", model);

            var result = await _service.ShowDialog("AddToDoView", param);
            if (result.Result == ButtonResult.OK)
                if (result.Parameters.ContainsKey("Value"))
                {
                    var todo = result.Parameters.GetValue<ToDoDto>("Value");
                    if (todo.Id > 0)
                    {
                        // 更新数据
                        var todoModel = await toDoService.UpdateAsync(todo);
                        if (todoModel.Status)
                        {
                            var todoFind = SummDto.ToDoList.FirstOrDefault(t => t.Id.Equals(todo.Id));
                            if (todoFind != null)
                            {
                                todoFind.Title = todo.Title;
                                todoFind.Content = todo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addResult = await toDoService.AddAsync(todo);
                        if (addResult.Status)
                            SummDto.ToDoList.Add(todo);
                    }
                }
        }

        private async void AddMemo(MemoDto model)
        {
            var param = new DialogParameters();
            if (model != null)
                param.Add("Value", model);

            var result = await _service.ShowDialog("AddMemoView", param);
            if (result.Result == ButtonResult.OK)
                if (result.Parameters.ContainsKey("Value"))
                {
                    var memo = result.Parameters.GetValue<MemoDto>("Value");
                    if (memo.Id > 0)
                    {
                        // 更新数据
                        var memoModel = await memoService.UpdateAsync(memo);
                        if (memoModel.Status)
                        {
                            var memoFind = SummDto.MemoList.FirstOrDefault(t => t.Id.Equals(memo.Id));
                            if (memoFind != null)
                            {
                                memoFind.Title = memo.Title;
                                memoFind.Content = memo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addResult = await memoService.AddAsync(memo);
                        if (addResult.Status)
                            SummDto.MemoList.Add(memo);
                    }
                }
        }

        private async void GetDataAsync()
        {
            var result =  await toDoService.GetSummayAsync();
            if (result.Status)
            {
                SummDto.ToDoList = result.Result.ToDoList;
                SummDto.MemoList = result.Result.MemoList;

                SummDto.ToDoCount = result.Result.ToDoCount;
                SummDto.CompletedCount = result.Result.CompletedCount;
                SummDto.CompletedRadio = result.Result.CompletedRadio;
                SummDto.MemoCount = result.Result.MemoCount;

                Refresh();
            }
            //// 创建list 列表数据
            //var todoResult = await toDoService.GetAllAsync(new ToDoParameter()
            //{
            //    PageIndex = 0,
            //    PageSize = 100
            //});
            //if (todoResult.Status)
            //{
            //    foreach (var item in todoResult.Result.Items)
            //    {
            //        SummDto.ToDoList.Add(item);
            //    }
            //}

            //// 创建 list 列表
            //var memoResult =await memoService.GetAllAsync(new QueryParameter()
            //{
            //    PageIndex = 0,
            //    PageSize = 100
            //});
            //if (memoResult.Status)
            //{
            //    foreach (var item in memoResult.Result.Items)
            //    {
            //        SummDto.MemoList.Add(item);
            //    }
            //}
        }

        private void Refresh()
        {
            // 更新
            taskBars[0].Content = SummDto.ToDoCount.ToString();
            taskBars[1].Content = SummDto.CompletedCount.ToString();
            taskBars[2].Content = SummDto.CompletedRadio;
            taskBars[3].Content = SummDto.MemoCount.ToString();
        }

        private void CreateTaskBar()
        {
            taskBars = new ObservableCollection<TaskBar>();
            taskBars.Add(new TaskBar()
            {
                Color = "#0ca0ff",
                Icon = "ClockFast",
                Title = "汇总",
                Target = "ToDoView"
            });
            taskBars.Add(new TaskBar()
            {
                Color = "#1eca3a",
                Icon = "ClockCheckOutline",
                Title = "已完成",
                Target = "ToDoView"
            });
            taskBars.Add(new TaskBar()
            {
                Color = "#02c6dc",
                Icon = "ChartLineVariant",
                Title = "完成比例",
                Target = ""
            });
            taskBars.Add(new TaskBar()
            {
                Color = "#ffa000",
                Icon = "PlaylistStar",
                Title = "备忘录",
                Target = "MemoView"
            });
        }

    }
}