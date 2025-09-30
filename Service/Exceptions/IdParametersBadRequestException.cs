namespace Service.Exceptions;
internal sealed class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException()
        : base("Parameter ids is null")
    {
    }
}
