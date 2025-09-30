using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace Presentation.Controllers;
[ApiController]
[Route("api/companies")]
public class CompaniesController(IServiceManager _service) : ControllerBase
{

    [HttpGet]
    public IActionResult GetCompanies()
    {
        var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }

    [HttpGet("collection/({ids})")]
    public IActionResult GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ModelBinders.ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = _service.CompanyService.GetByIds(ids, trackChanges: false);
        return Ok(companies);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CreateCompanyDto company)
    {
        var createdCompany = _service.CompanyService.CreateCompany(company);
        return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
    }

    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CreateCompanyDto> companyCollection)
    {
        var result = _service.CompanyService.CreateCompanyCollection(companyCollection);

        return CreatedAtAction(nameof(GetCompanyCollection), new { result.ids }, result.companies);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCompany(Guid id)
    {
        _service.CompanyService.DeleteCompany(id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateCompany(Guid id, [FromBody] UpdateCompanyDto company)
    {
        _service.CompanyService.UpdateCompany(id, company, trackChanges: true);
        return NoContent();
    }
}