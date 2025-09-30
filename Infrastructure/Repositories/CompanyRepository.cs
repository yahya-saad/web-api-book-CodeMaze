using Contracts;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;
internal class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToList();

    public Company? GetCompany(Guid companyId, bool trackChanges) =>
        FindByCondition(c => c.Id.Equals(companyId), trackChanges)
        .SingleOrDefault();

    public void CreateCompany(Company company) => Create(company);
    public void UpdateCompany(Company company) => Update(company);
    public void DeleteCompany(Company company) => Delete(company);

    public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(c => ids.Contains(c.Id), trackChanges)
        .ToList();


}
