using Contracts;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;
internal class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;


    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;
        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(context));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(context));
    }

    public ICompanyRepository Companies => _companyRepository.Value;

    public IEmployeeRepository Employees => _employeeRepository.Value;

    public void Dispose() => _context.Dispose();
    public void SaveChanges() => _context.SaveChanges();

}
