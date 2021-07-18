using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces.Repositories
{
    public interface IRegisterRepository
    {
        Task<UserEntity> GetRegisterByUserId(Guid id);
        Task<IEnumerable<UserEntity>> GetRegisters();
    }
}
