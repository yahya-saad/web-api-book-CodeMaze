namespace Shared.DTOs;
public record EmployeeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string Position { get; init; } = string.Empty;
}
