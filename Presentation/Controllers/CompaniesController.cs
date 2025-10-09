using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Service.Contracts;
using Shared.DTOs;

namespace Presentation.Controllers;
[ApiController]
[Route("api/companies")]
[ApiVersion(1)]
public class CompaniesController(IServiceManager _service) : ControllerBase
{

    [HttpOptions]
    public IActionResult GetCompaniesOptions()
    {
        Response.Headers.Append("Allow", "GET, OPTIONS, POST, PUT, DELETE");
        return Ok();
    }

    [HttpGet(Name = "GetCompanies")]
    //[ResponseCache(Duration = 60)]
    [OutputCache(Duration = 60)]
    [EndpointSummary("Get all companies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Get company by id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
        return Ok(company);
    }

    [HttpGet("collection/({ids})")]
    [EndpointSummary("Get company collection by ids")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ModelBinders.ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
        return Ok(companies);
    }

    [HttpPost(Name = "CreateCompany")]
    [EndpointSummary("Create a company")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto company)
    {
        var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
        return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
    }

    [HttpPost("collection")]
    [EndpointSummary("Create a collection of companies")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CreateCompanyDto> companyCollection)
    {
        var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);

        return CreatedAtAction(nameof(GetCompanyCollection), new { result.ids }, result.companies);
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Delete a company")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Update a company")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] UpdateCompanyDto company)
    {
        await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
        return NoContent();
    }
}