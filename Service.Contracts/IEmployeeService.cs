using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;
public interface IEmployeeService
{
    Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(
        Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<EmployeeDto?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, CreateEmployeeDto employee, bool trackChanges);
    Task DeleteEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    Task UpdateEmployeeAsync(Guid companyId, Guid id, UpdateeEmployeeDto employee
        , bool compTrackChanges, bool empTrackChanges);
}
