using Contracts;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;

namespace API.Middlewares;

public class BadRequestExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILoggerManager logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadRequestException badRequestException)
            return false;

        var problemDetails = new ProblemDetails
        {
            Title = "Bad Request",
            Detail = badRequestException.Message,
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception,
        };

        logger.LogWarn($"A bad request exception occurred: {badRequestException.Message}");
        return await problemDetailsService.TryWriteAsync(context);
    }
}
