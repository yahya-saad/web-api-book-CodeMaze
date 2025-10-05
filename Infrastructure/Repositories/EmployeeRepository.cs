using Contracts;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Infrastructure.Repositories;
internal class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
            .Search(employeeParameters.SearchTerm!)
            .Sort(employeeParameters.OrderBy!)
            .ToListAsync();

        return PagedList<Employee>.ToPagedList(
            employees,
            employeeParameters.PageNumber,
            employeeParameters.PageSize);
    }

    public async Task<Employee?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
        await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

    public void CreateEmployee(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee) => Delete(employee);


}
