using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using MyToDo.API.Context;
using MyToDo.Shared.Dtos;

namespace MyToDo.API.Extensions
{
    public class AutoMapperProfile : MapperConfigurationExpression
    {
        public AutoMapperProfile()
        {
            CreateMap<ToDoDto, ToDo>().ReverseMap();
            CreateMap<MemoDto, Memo>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
