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
    public class ToDoService : BaseService<ToDoDto>,IToDoService
    {
        private readonly HttpRestClient _client;

        public ToDoService(HttpRestClient client) : base(client, "ToDo")
        {
            _client = client;
        }

        public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllAsync(ToDoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;

            request.Route = $"api/ToDo/GetAll?PageIndex={parameter.PageIndex}&" +
                            $"PageSize={parameter.PageSize}&" +
                            $"Search={parameter.Search}&" + 
                            $"Status={parameter.Status}";
            return await _client.ExecuteAsync<PagedList<ToDoDto>>(request);
            //BaseRequest request = new BaseRequest();
            //request.Method = RestSharp.Method.GET;

            //request.Route = $"api/{_serviceName}/GetAll?PageIndex={parameter.PageIndex}&" +
            //                $"PageSize={parameter.PageSize}&" +
            //                $"Search={parameter.Search}";
            //return await _client.ExecuteAsync<PagedList<TEntity>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> GetSummayAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;

            request.Route = $"api/ToDo/GetSummary";
            return await _client.ExecuteAsync<SummaryDto>(request);
        }
    }
}
