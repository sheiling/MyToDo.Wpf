using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyToDo.Shared.Dtos;

namespace MyToDo.API.Services
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync(string account,string password);

        Task<ApiResponse> Register(UserDto user);
    }
}
