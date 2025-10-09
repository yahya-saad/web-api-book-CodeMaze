using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Presentation.Extensions;
using Service.Contracts;
using Shared.DTOs;

namespace Presentation.Controllers;
[ApiController]
[ApiVersion(2)]
[Route("api/v{v:apiversion}/companies")]
[ResponseCache(CacheProfileName = "120SecondsDuration")]
[EnableRateLimiting("SpecificPolicy")]
public class CompaniesV2Controller(IServiceManager service) : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetCompanies()
    {
        var result = await service.CompanyService
        .GetAllCompaniesAsync(trackChanges: false);

        var companies = result.GetResult<IEnumerable<CompanyDto>>();

        var response = companies.Select(c => $"{c.Name} v2");

        return Ok(response);
    }
}
