﻿using AuthIdentityJWT.SharedLibrary.Dtos;
using System.Linq.Expressions;

namespace AuthIdentityJWT.Core.Services
{
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<Response<TDto>> AddAsync(TDto tdto);
        Task<Response<NoDataDto>> Update(TDto tdto, int id);
        Task<Response<NoDataDto>> Remove(int id);
    }
}
