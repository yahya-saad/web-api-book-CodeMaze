namespace Shared.ApiResponses;
public abstract class ApiNotFoundResponse : ApiBaseResponse
{
    public string Message { get; set; }
    public ApiNotFoundResponse(string message)
    : base(false)
    {
        Message = message;
    }
}