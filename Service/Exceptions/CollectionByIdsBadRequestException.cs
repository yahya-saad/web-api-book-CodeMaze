namespace Service.Exceptions;
internal sealed class CollectionByIdsBadRequestException : BadRequestException
{
    public CollectionByIdsBadRequestException()
        : base("Collection count mismatch comparing to ids.")
    {
    }
}
