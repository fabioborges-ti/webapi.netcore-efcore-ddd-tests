using Api.Domain.Dtos.Address;
using Api.Domain.Dtos.Phone;
using Api.Domain.Dtos.User;
using Api.Domain.Models;
using AutoMapper;

namespace Api.CrossCutting.Mappings
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            // user
            CreateMap<UserModel, UserDto>().ReverseMap();
            CreateMap<UserModel, UserDtoCreate>().ReverseMap();
            CreateMap<UserModel, UserDtoUpdate>().ReverseMap();

            // address
            CreateMap<AddressModel, AddressDto>().ReverseMap();
            CreateMap<AddressModel, AddressDtoCreate>().ReverseMap();
            CreateMap<AddressModel, AddressDtoUpdate>().ReverseMap();

            // phone
            CreateMap<PhoneModel, PhoneDto>().ReverseMap();
            CreateMap<PhoneModel, PhoneDtoCreate>().ReverseMap();
            CreateMap<PhoneModel, PhoneDtoUpdate>().ReverseMap();
        }
    }
}
