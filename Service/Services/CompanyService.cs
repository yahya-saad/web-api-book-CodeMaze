using AutoMapper;
using Contracts;
using Domain.Entities;
using Service.Contracts;
using Service.Exceptions;
using Shared.ApiResponses;
using Shared.DTOs;

namespace Service.Services;
internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ApiBaseResponse> GetAllCompaniesAsync(bool trackChanges)
    {
        var companies = await _repository.Companies.GetAllCompaniesAsync(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return new ApiOkResponse<IEnumerable<CompanyDto>>(companiesDto);
    }

    public async Task<ApiBaseResponse> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Companies.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
            return new CompanyNotFoundResponse(companyId);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return new ApiOkResponse<CompanyDto>(companyDto);
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);
        _repository.Companies.CreateCompany(companyEntity);
        await _repository.SaveChangesAsync();
        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var companyEntities = await _repository.Companies.GetByIdsAsync(ids, trackChanges);

        if (ids.Count() != companyEntities.Count())
            throw new CollectionByIdsBadRequestException();

        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

        return companiesToReturn;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CreateCompanyDto> companyCollection)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequest();

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntities)
        {
            _repository.Companies.CreateCompany(company);
        }
        await _repository.SaveChangesAsync();

        var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

        return (companies: companyCollectionToReturn, ids: ids);
    }

    public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

        _repository.Companies.DeleteCompany(company);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateCompanyAsync(Guid companyId, UpdateCompanyDto company, bool trackChanges)
    {
        var companyEntity = await _repository.Companies.GetCompanyAsync(companyId, trackChanges);
        if (companyEntity is null)
            throw new CompanyNotFoundException(companyId);

        _mapper.Map(company, companyEntity);

        await _repository.SaveChangesAsync();
    }

    private async Task<Company> GetCompanyAndCheckIfItExists(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Companies.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        return company;
    }
}
