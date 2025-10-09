﻿namespace Shared.ApiResponses;
public abstract class ApiBadRequestResponse : ApiBaseResponse
{
    public string Message { get; set; }
    public ApiBadRequestResponse(string message)
    : base(false)
    {
        Message = message;
    }
}