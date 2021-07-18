using Api.Domain.Dtos.Phone;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services.Phone
{
    public interface IPhoneService
    {
        Task<PhoneDto> Get(Guid id);
        Task<IEnumerable<PhoneDto>> GetByUser(Guid id);
        Task<PhoneDtoCreateResult> Post(PhoneDtoCreate item);
        Task<PhoneDtoUpdateResult> Put(PhoneDtoUpdate item);
        Task<bool> Delete(Guid id);
    }
}
