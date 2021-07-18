using Api.Domain.Entities;
using Api.Domain.Interfaces.Repositories;
using Api.Domain.Interfaces.Services.Register;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Service.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IRegisterRepository _repository;
        private readonly IMapper _mapper;

        public RegisterService(IMapper mapper, IRegisterRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<UserEntity>> GetRegisters()
        {
            var users = await _repository.GetRegisters();

            return users == null ? null : _mapper.Map<IEnumerable<UserEntity>>(users);
        }

        public async Task<UserEntity> GetRegisterById(Guid id)
        {
            var user = await _repository.GetRegisterByUserId(id);

            return user == null ? null : _mapper.Map<UserEntity>(user);
        }
    }
}
