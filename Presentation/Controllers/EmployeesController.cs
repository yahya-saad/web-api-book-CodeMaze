using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;
using System.Text.Json;

namespace Presentation.Controllers;
[ApiController]
[Route("api/companies/{companyId:guid}/employees")]
[ApiVersion(1)]
public class EmployeesController(IServiceManager _service) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all employees for a company")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
    {
        var (employees, metaData) = await _service.EmployeeService.GetEmployeesAsync(
            companyId, employeeParameters, trackChanges: false);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Get a specific employee for a company")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
        if (employee == null)
            return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    [EndpointSummary("Create an employee for a company")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] CreateEmployeeDto employee)
    {
        var createdEmployee = await _service.EmployeeService.CreateEmployeeAsync(companyId, employee, trackChanges: false);
        return CreatedAtAction(nameof(GetEmployeeForCompany), new { companyId, id = createdEmployee.Id }, createdEmployee);
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Delete an employee for a company")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        await _service.EmployeeService.DeleteEmployeeAsync(companyId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Update an employee for a company")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] UpdateeEmployeeDto employee)
    {
        await _service.EmployeeService.UpdateEmployeeAsync(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true);
        return NoContent();
    }
}
