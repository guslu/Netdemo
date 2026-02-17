using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Netdemo.Application.Common.Exceptions;

namespace Netdemo.Api.Middleware;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = httpContext.TraceIdentifier;

        switch (exception)
        {
            case ValidationException validationException:
                await WriteValidationProblemDetailsAsync(httpContext, validationException, traceId, cancellationToken);
                return true;
            case NotFoundException:
                await WriteProblemDetailsAsync(httpContext, StatusCodes.Status404NotFound, "Resource was not found.", exception.Message, traceId, cancellationToken);
                return true;
            case ConflictException:
                await WriteProblemDetailsAsync(httpContext, StatusCodes.Status409Conflict, "Request conflict.", exception.Message, traceId, cancellationToken);
                return true;
            case ForbiddenException:
                await WriteProblemDetailsAsync(httpContext, StatusCodes.Status403Forbidden, "Forbidden.", exception.Message, traceId, cancellationToken);
                return true;
            case UnauthorizedAccessException:
                await WriteProblemDetailsAsync(httpContext, StatusCodes.Status401Unauthorized, "Unauthorized.", exception.Message, traceId, cancellationToken);
                return true;
            default:
                logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);
                await WriteProblemDetailsAsync(httpContext, StatusCodes.Status500InternalServerError, "An unexpected error occurred.", "An unexpected server error occurred.", traceId, cancellationToken);
                return true;
        }
    }

    private static Task WriteProblemDetailsAsync(
        HttpContext httpContext,
        int statusCode,
        string title,
        string detail,
        string traceId,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = traceId;
        return httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private static Task WriteValidationProblemDetailsAsync(
        HttpContext httpContext,
        ValidationException validationException,
        string traceId,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => string.IsNullOrWhiteSpace(g.Key) ? "request" : g.Key,
                g => g.Select(e => e.ErrorMessage).Distinct().ToArray());

        var validationProblemDetails = new ValidationProblemDetails(errors)
        {
            Title = "Request validation failed.",
            Detail = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Instance = httpContext.Request.Path
        };

        validationProblemDetails.Extensions["traceId"] = traceId;
        return httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);
    }
}
