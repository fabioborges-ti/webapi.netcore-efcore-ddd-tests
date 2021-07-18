using Api.Domain.Dtos.Phone;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repositories;
using Api.Domain.Interfaces.Services.Phone;
using Api.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Service.Services
{
    public class PhoneService : IPhoneService
    {
        private readonly ILogger<PhoneService> _logger;
        private readonly IMapper _mapper;
        private readonly IPhoneRepository _repository;

        public PhoneService(ILogger<PhoneService> logger, IMapper mapper, IPhoneRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<PhoneDtoCreateResult> Post(PhoneDtoCreate item)
        {
            try
            {
                var model = _mapper.Map<PhoneModel>(item);
                var entity = _mapper.Map<PhoneEntity>(model);

                var result = await _repository.InsertAsync(entity);

                return _mapper.Map<PhoneDtoCreateResult>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PhoneService)} - Error method -> [{nameof(Post)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }

        public async Task<PhoneDtoUpdateResult> Put(PhoneDtoUpdate item)
        {
            try
            {
                var data = await _repository.SelectAsync(item.Id);

                if (data == null || data.UserId != item.UserId) return null;

                var model = _mapper.Map<PhoneModel>(item);
                var entity = _mapper.Map<PhoneEntity>(model);
                
                var result = await _repository.UpdateAsync(entity);

                return _mapper.Map<PhoneDtoUpdateResult>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PhoneService)} - Error method -> [{nameof(Put)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var result = await _repository.ExistAsync(id);

                return result && await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PhoneService)} - Error method -> [{nameof(Delete)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }

        public async Task<PhoneDto> Get(Guid id)
        {
            try
            {
                var result = await _repository.SelectAsync(id);

                return result == null ? null : _mapper.Map<PhoneDto>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PhoneService)} - Error method -> [{nameof(Get)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }

        public async Task<IEnumerable<PhoneDto>> GetByUser(Guid id)
        {
            try
            {
                var result = await _repository.SelectByUserAsync(id);

                return result == null ? null : _mapper.Map<IEnumerable<PhoneDto>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PhoneService)} - Error method -> [{nameof(GetByUser)}];[{ex.Message}];[{ex.StackTrace}]");

                throw;
            }
        }
    }
}
