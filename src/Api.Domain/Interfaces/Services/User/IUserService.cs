using Api.Domain.Dtos.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<UserDtoCreateResult> Post(UserDtoCreate user);
        Task<UserDtoUpdateResult> Put(UserDtoUpdate user);
        Task<bool> Delete(Guid id);
        Task<UserDto> ChangeStatus(Guid id);
        Task<UserDto> Get(Guid id);
        Task<IEnumerable<UserDto>> GetAll();
    }
}
