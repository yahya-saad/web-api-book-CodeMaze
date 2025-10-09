namespace Shared;
// Static Factory Methods Pattern instead of Inheritance
public class ApiResponse
{
    public bool Success { get; private set; }
    public string? Message { get; private set; }
    public object? Data { get; private set; }

    private ApiResponse(bool success, object? data = null, string? message = null)
    {
        Success = success;
        Data = data;
        Message = message;
    }

    public static ApiResponse Ok<T>(T data) => new(true, data);

    public static ApiResponse Ok() => new(true);

    // Error generic
    public static ApiResponse NotFound(string message) => new(false, null, message);
    public static ApiResponse BadRequest(string message) => new(false, null, message);
}
