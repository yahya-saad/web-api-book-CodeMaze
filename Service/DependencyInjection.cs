using Microsoft.Extensions.DependencyInjection;
using Service.Contracts;
using Service.Services;

namespace Service;
public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IServiceManager, ServiceManager>();

        return services;
    }
}
