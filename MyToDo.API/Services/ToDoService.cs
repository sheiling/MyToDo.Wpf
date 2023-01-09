using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.API.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using AutoMapper;


namespace MyToDo.API.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            var todo = _mapper.Map<ToDo>(model);
            await _unitOfWork.GetRepository<ToDo>().InsertAsync(todo);
            if (await _unitOfWork.SaveChangesAsync() > 0)
                return new ApiResponse(true, todo);
            return new ApiResponse("添加数据失败");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(todo);

            if (await _unitOfWork.SaveChangesAsync() > 0)
                return new ApiResponse(true, "");
            return new ApiResponse("删除数据失败");
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todos = await repository.GetPagedListAsync(predicate:
                x=>string.IsNullOrWhiteSpace(parameter.Search)?true:x.Title.Contains(parameter.Search),
                pageIndex:parameter.PageIndex,
                pageSize:parameter.PageSize,
                orderBy:source=>source.OrderByDescending(t=>t.CreateDate));
            return new ApiResponse(true, todos);
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            return new ApiResponse(true, todo);
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            var dbTodo = _mapper.Map<ToDo>(model);
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbTodo.Id));
            if (todo != null)
            {
                todo.Content = dbTodo.Content;
                todo.Status = dbTodo.Status;
                todo.Title = dbTodo.Title;
                todo.UpdateTime = DateTime.Now;
            }
            repository.Update(dbTodo);
            if (await _unitOfWork.SaveChangesAsync() > 0)
                return new ApiResponse(true, dbTodo);
            return new ApiResponse("更新数据失败");
        }

        public async Task<ApiResponse> GetAllAsync(ToDoParameter parameter)
        {
            var repository = _unitOfWork.GetRepository<ToDo>();
            var todos = await repository.GetPagedListAsync(predicate:
                x => (string.IsNullOrWhiteSpace(parameter.Search) ? true :
                    x.Title.Contains(parameter.Search)) &&
                    (parameter.Status == null ? true : parameter.Status.Equals(x.Status)),
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                orderBy: source => source.OrderByDescending(t => t.CreateDate));
            return new ApiResponse(true, todos);
        }

        public async Task<ApiResponse> GetSummaryAsync()
        {
            // 得到待办事项 
            var todos = await _unitOfWork.GetRepository<ToDo>().GetPagedListAsync(orderBy: source => source.OrderByDescending(t => t.CreateDate));
            // 得到备忘录
            var memos = await _unitOfWork.GetRepository<Memo>().GetPagedListAsync(orderBy: source => source.OrderByDescending(t => t.CreateDate));

            var summaryDtos = new SummaryDto();
            summaryDtos.ToDoCount = todos.Items.Count;
            summaryDtos.CompletedCount = todos.Items.Count(t => t.Status == 1);
            summaryDtos.CompletedRadio = (summaryDtos.CompletedCount / ((double)summaryDtos.ToDoCount)).ToString("0%");
            summaryDtos.MemoCount = memos.Items.Count;

            summaryDtos.ToDoList = new ObservableCollection<ToDoDto>(_mapper.Map<List<ToDoDto>>(todos.Items.Where(t => t.Status == 0)));
            summaryDtos.MemoList = new ObservableCollection<MemoDto>(_mapper.Map<List<MemoDto>>(memos.Items));

            return new ApiResponse(true, summaryDtos);
        }
    }
}
