using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;


namespace MyToDo.Service
{
    public class LoginService : BaseService<UserDto>, ILoginService
    {
        private readonly HttpRestClient _client;

        public LoginService(HttpRestClient client) : base(client, "Login")
        {
            _client = client;
        }

        public async Task<ApiResponse<UserDto>> LoginAsync(UserDto dto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Parameter = dto;

            request.Route = $"api/Login/Login";
            return await _client.ExecuteAsync<UserDto>(request);
        }

        public async Task<ApiResponse<UserDto>> RegisterAsync(UserDto dto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Parameter = dto;
            request.Route = $"api/Login/Register";
            return await _client.ExecuteAsync<UserDto>(request);
        }
    }
}
