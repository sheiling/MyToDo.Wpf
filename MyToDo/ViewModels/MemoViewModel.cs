using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public class MemoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        private readonly IMemoService _service;
        private ObservableCollection<MemoDto> memoList;
        public ObservableCollection<MemoDto> MemoList
        {
            get => memoList;
            set
            {
                memoList = value;
                RaisePropertyChanged();
            }
        }

        private MemoDto currentDto;
        public MemoDto CurrentDto
        {
            get => currentDto;
            set
            {
                currentDto = value; 
                RaisePropertyChanged();
            }
        }


        private string search;
        public string Search
        {
            get => search;
            set
            {
                search = value;
                RaisePropertyChanged();
            }
        }

        private bool isRightOpen;
        public bool IsRightOpen
        {
            get => isRightOpen;
            set
            {
                isRightOpen = value;
                RaisePropertyChanged();
            }
        }
        
        public DelegateCommand<string> SearchCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }

        
        public MemoViewModel(IMemoService service, IContainerProvider provider) : base(provider)
        {
            _service = service;
            memoList = new ObservableCollection<MemoDto>();

            SearchCommand = new DelegateCommand<string>(OnSearch);
            ExecuteCommand = new DelegateCommand<string>(OnExecute);
            DeleteCommand = new DelegateCommand<MemoDto>(OnDelete);
            SelectedCommand = new DelegateCommand<MemoDto>(OnSelected);

            dialogHost = provider.Resolve<IDialogHostService>();
        }

        private void OnSelected(MemoDto obj)
        {
            try
            {
                IsRightOpen = true;
                CurrentDto = obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async void OnDelete(MemoDto memo)
        {
            try
            {
                var resultDialog = await dialogHost.Question("温馨提示", $"是否删除备忘录:{memo.Title} ？");
                if (resultDialog.Result != ButtonResult.OK)
                    return;

                UpdateLoading(true);
                var result = await _service.DeleteAsync(memo.Id);
                if (result.Status)
                {
                    var m = MemoList.FirstOrDefault(m => m.Id.Equals(memo.Id));
                    if (m != null)
                        MemoList.Remove(m);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private void OnExecute(string obj)
        {
            switch (obj)
            {
                case "添加":
                    Add();
                    break;
                case "保存":
                    Save();
                    break;
            }
        }

        private async void Save()
        {
            if(string.IsNullOrEmpty(currentDto.Title) || 
               string.IsNullOrEmpty(currentDto.Content))
                return;
            try
            {
                UpdateLoading(true);
                if (currentDto.Id > 0)
                {
                    // 查找是否存在 
                    var memoResult = await _service.UpdateAsync(currentDto);
                    if (memoResult.Status)
                    {
                        var memo = MemoList.FirstOrDefault(m => m.Id.Equals(memoResult.Result.Id));
                        if (memo != null)
                        {
                            memo.Title = memoResult.Result.Title;
                            memo.Content = memoResult.Result.Content;
                        }
                    }
                }
                else
                {
                    var result = await _service.AddAsync(currentDto);
                    if (result.Status)
                    {
                        MemoList.Add(currentDto);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsRightOpen = false;
                UpdateLoading(false);
            }
        }

        private void Add()
        {
            currentDto = new MemoDto();
            IsRightOpen = true;
        }

        private void OnSearch(string obj)
        {
             GetMemoDataAsync();
        }

        private async void GetMemoDataAsync()
        {
            try
            {
                UpdateLoading(true);
                var result = await _service.GetAllAsync(new QueryParameter()
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = Search
                });

                if (result.Status)
                {
                    memoList.Clear();
                    foreach (var item in result.Result.Items)
                    {
                        memoList.Add(item);
                    }
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
            GetMemoDataAsync();
        }
    }
}