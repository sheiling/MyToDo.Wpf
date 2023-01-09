using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyToDo.Shared.Parameters;

namespace MyToDo.API.Services
{
    public interface IBaseService<T>
    {
        public Task<ApiResponse> AddAsync(T model);
        public Task<ApiResponse> DeleteAsync(int id);
        public Task<ApiResponse> GetAllAsync(QueryParameter parameter);
        public Task<ApiResponse> GetSingleAsync(int id);
        public Task<ApiResponse> UpdateAsync(T model);
    }
}
