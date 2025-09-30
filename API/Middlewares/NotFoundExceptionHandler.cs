using Contracts;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Service.Exceptions;

namespace API.Middlewares;

public class NotFoundExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILoggerManager logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
            return false;

        var problemDetails = new ProblemDetails
        {
            Title = "Resourse Not Found",
            Detail = notFoundException.Message,
        };
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception,
        };

        logger.LogWarn($"A not found exception occurred: {notFoundException.Message}");
        return await problemDetailsService.TryWriteAsync(context);
    }
}
