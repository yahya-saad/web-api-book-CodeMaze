using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace Presentation.Controllers;
[ApiController]
[Route("api/companies/{companyId:guid}/employees")]
public class EmployeesController(IServiceManager _service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] CreateEmployeeDto employee)
    {
        var createdEmployee = _service.EmployeeService.CreateEmployee(companyId, employee, trackChanges: false);

        return CreatedAtAction(nameof(GetEmployeeForCompany), new { companyId, id = createdEmployee.Id }, createdEmployee);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployee(companyId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] UpdateeEmployeeDto employee)
    {
        _service.EmployeeService.UpdateEmployee(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true);
        return NoContent();
    }
}
