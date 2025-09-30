namespace Shared.DTOs;
public record UpdateCompanyDto(string Name, string Address,
    string Country, IEnumerable<CreateEmployeeDto> Employees);
