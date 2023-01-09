using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    public class SummaryDto : BaseDto
    {
        private int toDoCount;
        private int completedCount;
        private string completedRadio;
        private int memoCount;

        public int ToDoCount
        {
            get => toDoCount;
            set
            {
                toDoCount = value;
                OnPropertyChanged();
            }
        }

        public int CompletedCount
        {
            get => completedCount;
            set
            {
                completedCount = value;
                OnPropertyChanged();
            }
        }

        public string CompletedRadio
        {
            get => completedRadio;
            set
            {
                completedRadio = value;
                OnPropertyChanged();
            }
        }

        public int MemoCount
        {
            get => memoCount;
            set
            {
                memoCount = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<ToDoDto> toDoList;
        public ObservableCollection<ToDoDto> ToDoList
        {
            get => toDoList;
            set
            {
                toDoList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MemoDto> memoList;
        public ObservableCollection<MemoDto> MemoList
        {
            get => memoList;
            set
            {
                memoList = value;
                OnPropertyChanged();
            }
        }

    }
}
