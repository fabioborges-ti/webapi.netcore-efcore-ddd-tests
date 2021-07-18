using Api.Domain.Dtos.Address;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services.Address
{
    public interface IAddressService
    {
        Task<AddressDto> Get(Guid id);
        Task<IEnumerable<AddressDto>> GetByUser(Guid id);
        Task<AddressDtoCreateResult> Post(AddressDtoCreate item);
        Task<AddressDtoUpdateResult> Put(AddressDtoUpdate item);
        Task<bool> Delete(Guid id);
    }
}
