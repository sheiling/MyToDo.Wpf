using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyToDo.API.Context;
using MyToDo.API.Services;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto model) => await _service.LoginAsync(model.Name, model.Password);


        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto model) => await _service.Register(model);

       
    }
}
