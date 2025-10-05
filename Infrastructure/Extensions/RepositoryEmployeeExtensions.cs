using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;
public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(
        this IQueryable<Employee> employees, uint minAge, uint maxAge) =>
employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));

    public static IQueryable<Employee> Search(
        this IQueryable<Employee> employees, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return employees;

        searchTerm = searchTerm.Trim();

        return employees.Where(e =>
            EF.Functions.Like(e.Name, $"%{searchTerm}%"));
    }

}
