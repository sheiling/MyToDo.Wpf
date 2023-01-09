using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.API.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;


namespace MyToDo.API.Services
{
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemoService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            var memo = _mapper.Map<Memo>(model);
            await _unitOfWork.GetRepository<Memo>().InsertAsync(memo);
            if (await _unitOfWork.SaveChangesAsync() > 0)
                return new ApiResponse(true, memo);
            return new ApiResponse("添加数据失败");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            repository.Delete(memo);

            if (await _unitOfWork.SaveChangesAsync() > 0)
                return new ApiResponse(true, "");
            return new ApiResponse("删除数据失败");
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            var memos = await repository.GetPagedListAsync(predicate:
                x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                orderBy: source => source.OrderByDescending(t => t.CreateDate));
            return new ApiResponse(true, memos);
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Memo>();
            var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
            return new ApiResponse(true, memo);
        }

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            var dbMemo = _mapper.Map<Memo>(model);
            var repository = _unitOfWork.GetRepository<Memo>();
            var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbMemo.Id));

            todo.Content = dbMemo.Content;
            todo.Title = dbMemo.Title;
            todo.UpdateTime = DateTime.Now;

            repository.Update(todo);
            if (await _unitOfWork.SaveChangesAsync() > 0)
                return new ApiResponse(true, todo);
            return new ApiResponse("更新数据失败");
        }
    }
}
