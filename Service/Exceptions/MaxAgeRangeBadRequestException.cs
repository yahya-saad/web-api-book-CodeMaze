﻿namespace Service.Exceptions;
internal class MaxAgeRangeBadRequestException : BadRequestException
{
    public MaxAgeRangeBadRequestException()
        : base("Max age can't be less than min age.")
    {
    }
}
