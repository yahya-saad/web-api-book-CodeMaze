using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace Presentation.Controllers;
[ApiController]
[Route("api/companies/{companyId:guid}/employees")]
public class EmployeesController(IServiceManager _service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
    {
        var employees = await _service.EmployeeService.GetEmployeesAsync(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, id, trackChanges: false);
        if (employee == null)
            return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] CreateEmployeeDto employee)
    {
        var createdEmployee = await _service.EmployeeService.CreateEmployeeAsync(companyId, employee, trackChanges: false);
        return CreatedAtAction(nameof(GetEmployeeForCompany), new { companyId, id = createdEmployee.Id }, createdEmployee);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        await _service.EmployeeService.DeleteEmployeeAsync(companyId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] UpdateeEmployeeDto employee)
    {
        await _service.EmployeeService.UpdateEmployeeAsync(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true);
        return NoContent();
    }
}
