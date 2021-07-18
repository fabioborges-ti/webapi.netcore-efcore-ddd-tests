using Api.Domain.Dtos.Address;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repositories;
using Api.Domain.Interfaces.Services.Address;
using Api.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly ILogger<AddressService> _logger;
        private readonly IMapper _mapper;
        private readonly IAddressRepository _repository;

        public AddressService(ILogger<AddressService> logger, IMapper mapper, IAddressRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<AddressDtoCreateResult> Post(AddressDtoCreate item)
        {
            try
            {
                var model = _mapper.Map<AddressModel>(item);
                var entity = _mapper.Map<AddressEntity>(model);

                var result = await _repository.InsertAsync(entity);

                return _mapper.Map<AddressDtoCreateResult>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error method -> [{nameof(Post)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }

        public async Task<AddressDtoUpdateResult> Put(AddressDtoUpdate item)
        {
            try
            {
                var data = await _repository.SelectAsync(item.Id);

                if (data == null || data.UserId != item.UserId) return null;

                var model = _mapper.Map<AddressModel>(item);
                var entity = _mapper.Map<AddressEntity>(model);
                
                var result = await _repository.UpdateAsync(entity);

                return _mapper.Map<AddressDtoUpdateResult>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error method -> [{nameof(Put)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var data = await _repository.ExistAsync(id);

                return data && await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error method -> [{nameof(Delete)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }

        public async Task<AddressDto> Get(Guid id)
        {
            try
            {
                var entity = await _repository.SelectAsync(id);

                return entity == null ? null : _mapper.Map<AddressDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error method -> [{nameof(Get)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }

        public async Task<IEnumerable<AddressDto>> GetByUser(Guid id)
        {
            try
            {
                var entity = await _repository.SelectByUserAsync(id);

                return !entity.Any() ? null : _mapper.Map<IEnumerable<AddressDto>>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error method -> [{nameof(GetByUser)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }
    }
}
