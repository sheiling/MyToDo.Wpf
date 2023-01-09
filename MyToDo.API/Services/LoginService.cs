using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.API.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Extensions;


namespace MyToDo.API.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoginService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> LoginAsync(string name, string password)
        {
            password = password.GetMD5();
            var repository = _unitOfWork.GetRepository<User>();
            var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.UserName.Equals(name)
                                                                               && x.PassWord.Equals(password));
            if (user != null)
                return new ApiResponse(true, user);

            return new ApiResponse("登录失败!");
        }

        public async Task<ApiResponse> Register(UserDto user)
        {
            var ser = _mapper.Map<User>(user);
            var repository = _unitOfWork.GetRepository<User>();

            var model = await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(ser.Account));
            if (model != null)
                return new ApiResponse($"用户名 {ser.Account} 已经存在，请重新注册！");

            ser.UserName = user.Name;
            ser.PassWord = user.Password.GetMD5();
            ser.CreateDate = DateTime.Now;
            await repository.InsertAsync(ser);
            if (await _unitOfWork.SaveChangesAsync() > 0)
                return new ApiResponse(true, ser);

            return new ApiResponse("注册用户名失败！");
        }
    }
}
