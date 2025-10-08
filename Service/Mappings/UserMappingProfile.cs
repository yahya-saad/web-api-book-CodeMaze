using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace Service.Mappings;
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserDto, User>();
    }
}
