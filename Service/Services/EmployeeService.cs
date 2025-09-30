using AutoMapper;
using Contracts;
using Domain.Entities;
using Service.Contracts;
using Service.Exceptions;
using Shared.DTOs;

namespace Service.Services;
internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public EmployeeDto? GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repository.Companies.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = _repository.Employees.GetEmployee(companyId, id, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(id);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;

    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repository.Companies.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employees = _repository.Employees.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return employeesDto;
    }

    public EmployeeDto CreateEmployee(Guid companyId, CreateEmployeeDto employee, bool trackChanges)
    {
        var company = _repository.Companies.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = _mapper.Map<Employee>(employee);
        _repository.Employees.CreateEmployee(companyId, employeeEntity);
        _repository.SaveChanges();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;

    }

    public void DeleteEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repository.Companies.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = _repository.Employees.GetEmployee(companyId, id, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(id);

        _repository.Employees.DeleteEmployee(employee);
        _repository.SaveChanges();
    }

    public void UpdateEmployee(
        Guid companyId, Guid id, UpdateeEmployeeDto employee,
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = _repository.Companies.GetCompany(companyId, compTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = _repository.Employees.GetEmployee(companyId, id, empTrackChanges);
        if (employeeEntity is null)
            throw new EmployeeNotFoundException(id);

        _mapper.Map(employee, employeeEntity);
        _repository.SaveChanges();
    }
}
