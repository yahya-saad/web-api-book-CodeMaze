using Service.Contracts;

namespace Service.Services;
public sealed class ServiceManager : IServiceManager
{
    public ServiceManager(ICompanyService companyService, IEmployeeService employeeService)
    {
        CompanyService = companyService;
        EmployeeService = employeeService;
    }

    public ICompanyService CompanyService { get; }
    public IEmployeeService EmployeeService { get; }
}

