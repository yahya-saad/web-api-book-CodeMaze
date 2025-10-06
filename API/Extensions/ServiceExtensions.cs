namespace CompanyEmployees.Extensions;
using Asp.Versioning;
public static class ServiceExtensions
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.ConfigureCors();
        services.ConfigureIISIntegration();
        services.ConfigureVersioning();
        services.ConfigureResponseCaching();

        return services;
    }
    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination")
                    );
        });
    }

    private static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options =>
        {
        });
    }

    private static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
        }).AddMvc();
    }

    private static void ConfigureResponseCaching(this IServiceCollection services)
    {
        services.AddResponseCaching();
    }
}
