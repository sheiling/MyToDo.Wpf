using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyToDo.API.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;


namespace MyToDo.API.Services
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
        public Task<ApiResponse> GetAllAsync(ToDoParameter parameter);

        public Task<ApiResponse> GetSummaryAsync();
    }
}
