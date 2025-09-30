using Contracts;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Middlewares;

public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILoggerManager logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "Internal Server Error",
            Detail = "An error occurred while processing your request.",
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception,
        };

        logger.LogError($"An unhandled exception occurred: {exception.Message}");

        return await problemDetailsService.TryWriteAsync(context);
    }
}
