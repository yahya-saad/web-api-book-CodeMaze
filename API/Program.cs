using API.Extensions;
using CompanyEmployees.Extensions;
using Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");
app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
