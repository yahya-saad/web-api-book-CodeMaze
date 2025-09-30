namespace Service.Mappings;
using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => $"{src.Address}, {src.Country}"));

        CreateMap<CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>();
    }
}
