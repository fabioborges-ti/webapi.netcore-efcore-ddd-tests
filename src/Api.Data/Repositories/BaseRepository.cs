using Api.Data.Context;
using Api.Domain.Entities.Base;
using Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Api.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DataContext Context;
        
        private readonly ILogger _logger;
        
        private readonly DbSet<TEntity> _dataSet;

        public BaseRepository(ILogger logger, DataContext context)
        {
            Context = context;

            _logger = logger;
            _dataSet = Context.Set<TEntity>();
        }

        public async Task<TEntity> InsertAsync(TEntity item)
        {
            try
            {
                await _dataSet.AddAsync(item);
                await Context.SaveChangesAsync();
                
                return item;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                
                throw;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity item)
        {
            try
            {
                var result = await _dataSet.SingleOrDefaultAsync(p => p.Id.Equals(item.Id));

                Context.Entry(result).CurrentValues.SetValues(item);

                await Context.SaveChangesAsync();

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _dataSet.SingleOrDefaultAsync(p => p.Id.Equals(id));

                if (result == null)
                {
                    return false;
                }

                _dataSet.Remove(result);

                await Context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            try
            {
                var result = await _dataSet.AnyAsync(p => p.Id.Equals(id));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<TEntity> SelectAsync(Guid id)
        {
            try
            {
                var result = await _dataSet.SingleOrDefaultAsync(p => p.Id.Equals(id));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> SelectAsync()
        {
            try
            {
                var result = await _dataSet.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }
    }
}
