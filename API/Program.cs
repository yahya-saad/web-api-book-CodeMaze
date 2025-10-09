using API.Extensions;
using Asp.Versioning.ApiExplorer;
using CompanyEmployees.Extensions;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Scalar.AspNetCore;
using Service;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddApiConfiguration();
builder.Services.AddInfrastructure(builder.Configuration)
    .AddBusinessLogic();

builder.Services.AddExceptionHandlers();

builder.Services.AddControllers(cfg =>
{
    cfg.RespectBrowserAcceptHeader = true;
    cfg.ReturnHttpNotAcceptable = true;
    cfg.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
})
.AddXmlDataContractSerializerFormatters()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);


var app = builder.Build();

app.UseExceptionHandler();
if (app.Environment.IsProduction())
    app.UseHsts();

await app.ApplyMigrationsAsync<ApplicationDbContext>();

if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
            );
            options.DocumentTitle = "CodeMaze Docs";
        }
    });

    app.MapScalarApiReference(options =>
    {
        options.AddDocument("v1", "Production API", "/swagger/v1/swagger.json")
               .AddDocument("v2", "Beta API", "/swagger/v2/swagger.json");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseRateLimiter();
app.UseCors("CorsPolicy");
//app.UseResponseCaching();
app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
