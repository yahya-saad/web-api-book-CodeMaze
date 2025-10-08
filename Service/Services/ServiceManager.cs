using Service.Contracts;

namespace Service.Services;
public sealed class ServiceManager : IServiceManager
{
    public ServiceManager(
        ICompanyService companyService, IEmployeeService employeeService,
        IAuthenticationService authenticationService)
    {
        CompanyService = companyService;
        EmployeeService = employeeService;
        AuthenticationService = authenticationService;
    }

    public ICompanyService CompanyService { get; }
    public IEmployeeService EmployeeService { get; }
    public IAuthenticationService AuthenticationService { get; }
}

