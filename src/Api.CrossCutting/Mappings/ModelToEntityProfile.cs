using Api.Domain.Entities;
using Api.Domain.Models;
using AutoMapper;

namespace Api.CrossCutting.Mappings
{
    public class ModelToEntityProfile : Profile
    {
        public ModelToEntityProfile()
        {
            CreateMap<UserEntity, UserModel>().ReverseMap();
            CreateMap<AddressEntity, AddressModel>().ReverseMap();
            CreateMap<PhoneEntity, PhoneModel>().ReverseMap();
        }
    }
}
