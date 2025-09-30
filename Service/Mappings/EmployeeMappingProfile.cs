using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace Service.Mappings;
public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateeEmployeeDto, Employee>();
    }
}
