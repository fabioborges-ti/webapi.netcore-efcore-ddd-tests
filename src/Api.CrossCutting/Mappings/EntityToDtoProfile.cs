using Api.Domain.Dtos.Address;
using Api.Domain.Dtos.Phone;
using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using AutoMapper;

namespace Api.CrossCutting.Mappings
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            // user
            CreateMap<UserEntity, UserDto>().ReverseMap();
            CreateMap<UserEntity, UserDtoCreateResult>().ReverseMap();
            CreateMap<UserEntity, UserDtoUpdateResult>().ReverseMap();

            // address
            CreateMap<AddressEntity, AddressDto>().ReverseMap();
            CreateMap<AddressEntity, AddressDtoCreateResult>().ReverseMap();
            CreateMap<AddressEntity, AddressDtoUpdateResult>().ReverseMap();

            // phone
            CreateMap<PhoneEntity, PhoneDto>().ReverseMap();
            CreateMap<PhoneEntity, PhoneDtoCreateResult>().ReverseMap();
            CreateMap<PhoneEntity, PhoneDtoUpdateResult>().ReverseMap();
        }
    }
}
