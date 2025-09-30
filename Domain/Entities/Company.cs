using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
public class Company
{
    [Column("CompanyId")]
    public Guid Id { get; set; }

    [Required, MaxLength(60)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string Address { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
