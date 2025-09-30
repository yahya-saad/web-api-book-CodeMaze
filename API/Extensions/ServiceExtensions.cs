namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.ConfigureCors();
        services.ConfigureIISIntegration();

        return services;
    }
    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    private static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options =>
        {
        });
    }

}
