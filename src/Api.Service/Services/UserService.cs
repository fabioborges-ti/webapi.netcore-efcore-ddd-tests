using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Repositories;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Models;
using Api.Domain.Validations.Dtos.User;
using Api.Service.Notifications;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Service.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly INotifier _notifier;
        private readonly IMapper _mapper;
        private readonly IRepository<UserEntity> _repository;
        private readonly IUserRepository _userRepository;
        
        public UserService(ILogger<UserService> logger, INotifier notifier, IMapper mapper, IRepository<UserEntity> repository, IUserRepository userRepository)
        {
            _logger = logger;
            _notifier = notifier;
            _mapper = mapper;
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<UserDtoCreateResult> Post(UserDtoCreate user)
        {
            var validator = new UserDtoCreateValidator();
            var validationResult = await validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                _logger.LogError("UserDtoCreate is invalid;");

                _notifier.SetNotification(validationResult);

                return null;
            }

            var data = await _userRepository.GetByEmail(user.Email);

            if (data != null)
            {
                _logger.LogError("Email already registered.");

                _notifier.SetNotification(new Notification("Email already registered."));

                return null;
            }

            var model = _mapper.Map<UserModel>(user);
            var entity = _mapper.Map<UserEntity>(model);

            entity.CreatedAt = DateTime.UtcNow;
            entity.Status = true;

            var result = await _repository.InsertAsync(entity);

            return _mapper.Map<UserDtoCreateResult>(result);
        }

        public async Task<UserDtoUpdateResult> Put(UserDtoUpdate user)
        {
            var validator = new UserDtoUpdateValidator();
            var validationResult = await validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                _logger.LogError("validation failed");

                _notifier.SetNotification(validationResult);

                return null;
            }

            var model = _mapper.Map<UserModel>(user);
            var entity = _mapper.Map<UserEntity>(model);

            var data = await _repository.SelectAsync(user.Id);

            if (data == null) return null;

            entity.CreatedAt = data.CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(entity);

            _logger.LogInformation("updated");

            return _mapper.Map<UserDtoUpdateResult>(result);
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notifier.SetNotification(new Notification("UserId must to be informed."));

                return false;
            }

            var exists = await _repository.ExistAsync(id);

            if (!exists) return false;

            _logger.LogInformation($"User removed successfully.");

            return await _repository.DeleteAsync(id);
        }

        public async Task<UserDto> ChangeStatus(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogError("Invalid UserId");

                _notifier.SetNotification(new Notification("UserId must to be informed."));

                return null;
            }

            var user = await _repository.SelectAsync(userId);

            if (user == null)
            {
                _logger.LogInformation("UserId not found");

                return null;
            }

            if (!user.Status)
            {
                user.Activate();
            }
            else
            {
                user.Inactivate();
            }

            var model = _mapper.Map<UserModel>(user);
            var entity = _mapper.Map<UserEntity>(model);

            entity.CreatedAt = user.CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(entity);

            return _mapper.Map<UserDto>(result);
        }

        public async Task<UserDto> Get(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogError("UserId must to be informed.");

                _notifier.SetNotification(new Notification("UserId must to be informed."));

                return null;
            }

            var entity = await _repository.SelectAsync(userId);

            if (entity == null)
            {
                _logger.LogInformation("User not found;");

                _notifier.SetNotification(new Notification("User not found;"));
                
                return null;
            }

            var user = _mapper.Map<UserDto>(entity);

            return user;
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var result = await _repository.SelectAsync();

            return _mapper.Map<IEnumerable<UserDto>>(result);
        }
    }
}
