using AutoMapper;
using Contracts;
using Domain.Entities;
using Service.Contracts;
using Service.Exceptions;
using Shared.DTOs;
using Shared.RequestFeatures;

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

    public async Task<EmployeeDto?> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employee = await GetEmployeeForCompanyAndCheckIfItExist(companyId, id, trackChanges);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId, trackChanges);

        var employeesWithMetaData = await _repository.Employees.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData.Items);

        return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, CreateEmployeeDto employee, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeEntity = _mapper.Map<Employee>(employee);
        _repository.Employees.CreateEmployee(companyId, employeeEntity);
        await _repository.SaveChangesAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task DeleteEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employee = await GetEmployeeForCompanyAndCheckIfItExist(companyId, id, trackChanges);

        _repository.Employees.DeleteEmployee(employee);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(
        Guid companyId, Guid id, UpdateeEmployeeDto employee,
        bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);

        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExist(companyId, id, empTrackChanges);

        _mapper.Map(employee, employeeEntity);
        await _repository.SaveChangesAsync();
    }

    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Companies.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExist
        (Guid companyId, Guid id, bool trackChanges)
    {
        var employee = await _repository.Employees.GetEmployeeAsync(companyId, id, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(id);
        return employee;
    }
}
