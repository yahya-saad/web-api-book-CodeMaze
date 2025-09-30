using Shared.DTOs;

namespace Service.Contracts;
public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDto? GetEmployee(Guid companyId, Guid id, bool trackChanges);
    EmployeeDto CreateEmployee(Guid companyId, CreateEmployeeDto employee, bool trackChanges);
    void DeleteEmployee(Guid companyId, Guid id, bool trackChanges);
    void UpdateEmployee(Guid companyId, Guid id, UpdateeEmployeeDto employee
        , bool compTrackChanges, bool empTrackChanges);
}
