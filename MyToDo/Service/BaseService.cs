using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Shared;
using MyToDo.Shared.Parameters;


namespace MyToDo.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly HttpRestClient _client;
        private readonly string _serviceName;

        public BaseService(HttpRestClient client, string serviceName)
        {
            _client = client;
            _serviceName = serviceName;
        }

        public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;

            request.Route = $"api/{_serviceName}/Add";
            request.Parameter = entity;
           return await _client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;

            request.Route = $"api/{_serviceName}/Update";
            request.Parameter = entity;
            return await _client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.DELETE;

            request.Route = $"api/{_serviceName}/Delete?id={id}";
            return await _client.ExecuteAsync(request);
        }

        public async Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;

            request.Route = $"api/{_serviceName}/Get?id={id}";
            return await _client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;

            request.Route = $"api/{_serviceName}/GetAll?PageIndex={parameter.PageIndex}&" +
                            $"PageSize={parameter.PageSize}&" +
                            $"Search={parameter.Search}";
            return await _client.ExecuteAsync<PagedList<TEntity>>(request);
        }
    }
}
