using Api.Data.Context;
using Api.Data.Repositories;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Implementations
{
    [ExcludeFromCodeCoverage]
    public class AddressImplementations : BaseRepository<AddressEntity>, IAddressRepository
    {
        private readonly DbSet<AddressEntity> _dataSet;

        public AddressImplementations(ILogger logger, DataContext context) : base(logger, context)
        {
            _dataSet = context.Set<AddressEntity>();
        }

        public async Task<IEnumerable<AddressEntity>> SelectByUserAsync(Guid id)
        {
            return await _dataSet.AsNoTracking().Where(u => u.UserId.Equals(id)).ToListAsync();
        }
    }
}
