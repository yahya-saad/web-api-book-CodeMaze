namespace Contracts;
public interface IRepositoryManager : IDisposable
{
    ICompanyRepository Companies { get; }
    IEmployeeRepository Employees { get; }
    void SaveChanges();
}
