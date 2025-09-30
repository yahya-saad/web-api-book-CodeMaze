namespace Shared.DTOs;
public record CompanyDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string FullAddress { get; init; } = string.Empty;
}
