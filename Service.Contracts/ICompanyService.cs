using Shared.DTOs;

namespace Service.Contracts;
public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    CompanyDto? GetCompany(Guid companyId, bool trackChanges);
    CompanyDto CreateCompany(CreateCompanyDto company);
    IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection
        (IEnumerable<CreateCompanyDto> companyCollection);

    void DeleteCompany(Guid companyId, bool trackChanges);
    void UpdateCompany(Guid companyId, UpdateCompanyDto company, bool trackChanges);
}
