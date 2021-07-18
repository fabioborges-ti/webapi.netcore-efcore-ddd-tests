using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces.Repositories
{
    public interface IPhoneRepository : IRepository<PhoneEntity>
    {
        Task<IEnumerable<PhoneEntity>> SelectByUserAsync(Guid id);
    }
}
