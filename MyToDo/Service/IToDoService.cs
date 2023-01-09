using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;


namespace MyToDo.Service
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
        public Task<ApiResponse<PagedList<ToDoDto>>> GetAllAsync(ToDoParameter parameter);
        public Task<ApiResponse<SummaryDto>> GetSummayAsync();
    }
}
