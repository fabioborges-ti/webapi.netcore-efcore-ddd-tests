using Api.Data.Context;
using Api.Data.Repositories;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Api.Data.Implementations
{
    [ExcludeFromCodeCoverage]
    public class PhoneImplementations : BaseRepository<PhoneEntity>, IPhoneRepository
    {
        private readonly DbSet<PhoneEntity> _dataSet;

        public PhoneImplementations(ILogger logger, DataContext context) : base(logger, context)
        {
            _dataSet = context.Set<PhoneEntity>();
        }

        public async Task<IEnumerable<PhoneEntity>> SelectByUserAsync(Guid id)
        {
            return await _dataSet.AsNoTracking().Where(u => u.UserId.Equals(id)).ToListAsync();
        }
    }
}
