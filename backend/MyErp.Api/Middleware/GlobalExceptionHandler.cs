using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyErp.Common.Exceptions;
using MyErp.Common.Results;

namespace MyErp.Api.Middleware;

/// <summary>
/// Turns the exceptions the modules raise into RFC 7807 problem responses, so no
/// module has to know anything about HTTP status codes.
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title, code) = Map(exception);

        if (status >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception while processing {Path}.", httpContext.Request.Path);
        }
        else
        {
            logger.LogInformation(
                "Request to {Path} failed with {Code}: {Message}",
                httpContext.Request.Path,
                code,
                exception.Message);
        }

        httpContext.Response.StatusCode = status;

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = status >= StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred. Please try again."
                : exception.Message,
            Type = $"https://httpstatuses.io/{status}"
        };

        problemDetails.Extensions["code"] = code;

        if (exception is ValidationException { Errors.Count: > 0 } validation)
        {
            problemDetails.Extensions["errors"] = validation.Errors;
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        });
    }

    private static (int Status, string Title, string Code) Map(Exception exception) => exception switch
    {
        NotFoundException e => (StatusCodes.Status404NotFound, "Resource not found", e.Code),
        ValidationException e => (StatusCodes.Status400BadRequest, "Validation failed", e.Code),
        ConflictException e => (StatusCodes.Status409Conflict, "Conflict", e.Code),
        BusinessRuleException e => (StatusCodes.Status422UnprocessableEntity, "Business rule violated", e.Code),
        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", ErrorCodes.Unauthorized),
        _ => (StatusCodes.Status500InternalServerError, "Server error", ErrorCodes.Unexpected)
    };
}
