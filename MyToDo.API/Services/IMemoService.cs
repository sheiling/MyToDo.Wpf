using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyToDo.API.Context;
using MyToDo.Shared.Dtos;


namespace MyToDo.API.Services
{
    public interface IMemoService : IBaseService<MemoDto>
    {
    }
}
