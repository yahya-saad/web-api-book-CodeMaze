using Shared.ApiResponses;
using Shared.DTOs;
namespace Service.Contracts;
public interface ICompanyService
{
    Task<ApiBaseResponse> GetAllCompaniesAsync(bool trackChanges);
    Task<ApiBaseResponse> GetCompanyAsync(Guid companyId, bool trackChanges);
    Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto company);
    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CreateCompanyDto> companyCollection);
    Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
    Task UpdateCompanyAsync(Guid companyId, UpdateCompanyDto company, bool trackChanges);
}
