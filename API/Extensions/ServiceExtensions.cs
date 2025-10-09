namespace CompanyEmployees.Extensions;

using API.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.ConfigureCors();
        services.ConfigureIISIntegration();
        services.ConfigureVersioning();
        services.ConfigureResponseCaching();
        services.ConfigureRateLimitingOptions();
        services.ConfigureSwagger();

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
            opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

    }

    private static void ConfigureResponseCaching(this IServiceCollection services)
    {
        //services.AddResponseCaching();
        services.AddOutputCache(opt =>
        {
            opt.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
            opt.AddPolicy("ShortTerm", policy => policy.Expire(TimeSpan.FromSeconds(30)));
            opt.AddPolicy("LongTerm", policy => policy.Expire(TimeSpan.FromMinutes(5)));
        });
    }

    private static void ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        services.AddRateLimiter(opt =>
        {
            opt.OnRejected = async (context, token) =>
            {
                var retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var metadata)
                    ? metadata.TotalSeconds
                    : (double?)null;

                var message = retryAfter is not null
                        ? $"You have exceeded the allowed number of requests. Try again after {retryAfter} second(s)."
                        : "You have exceeded the allowed number of requests. Try again later.";

                var problemDetails = new ProblemDetails
                {
                    Title = "Too Many Requests",
                    Status = StatusCodes.Status429TooManyRequests,
                    Detail = message,
                    Instance = context.HttpContext.Request.Path
                };

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/problem+json";

                await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, token);
            };


            opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        QueueLimit = 2,
                        Window = TimeSpan.FromMinutes(1),
                    }
                )
            );

            opt.AddPolicy("SpecificPolicy", context =>
                RateLimitPartition.GetFixedWindowLimiter("SpecificLimiter",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 30,
                        Window = TimeSpan.FromMinutes(1),
                    }
                )
            );

        });
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme.",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
            });

            o.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                 }
             });
        });
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();

    }

}
