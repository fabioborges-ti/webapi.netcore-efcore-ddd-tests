using Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services.Register
{
    public interface IRegisterService
    {
        Task<UserEntity> GetRegisterById(Guid id);
        Task<IEnumerable<UserEntity>> GetRegisters();
    }
}
