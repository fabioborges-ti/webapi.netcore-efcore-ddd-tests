using Api.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> InsertAsync(TEntity obj);
        Task<TEntity> UpdateAsync(TEntity obj);
        Task<bool> DeleteAsync(Guid id);
        Task<TEntity> SelectAsync(Guid id);
        Task<IEnumerable<TEntity>> SelectAsync();
        Task<bool> ExistAsync(Guid id);
    }
}
