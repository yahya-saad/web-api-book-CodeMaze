using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service.Contracts;

namespace Presentation.Controllers;
[ApiController]
[ApiVersion(2)]
[Route("api/v{v:apiversion}/companies")]
[ResponseCache(CacheProfileName = "120SecondsDuration")]
[EnableRateLimiting("SpecificPolicy")]
public class CompaniesV2Controller(IServiceManager service) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await service.CompanyService
        .GetAllCompaniesAsync(trackChanges: false);

        var companiesV2 = companies.Select(x => $"{x.Name} V2");

        return Ok(companiesV2);
    }
}
