using Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Repositories
{
    public interface IAddressRepository : IRepository<AddressEntity>
    {
        Task<IEnumerable<AddressEntity>> SelectByUserAsync(Guid id);
    }
}
