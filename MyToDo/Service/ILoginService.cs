using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Service
{
    public interface ILoginService 
    {
        public Task<ApiResponse<UserDto>> LoginAsync(UserDto dto);
        public Task<ApiResponse<UserDto>> RegisterAsync(UserDto dto);
    }
}
