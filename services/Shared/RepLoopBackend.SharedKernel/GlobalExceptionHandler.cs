using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.SharedKernel;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is OperationCanceledException)
        {
            _logger.LogDebug("Request cancelled: [{Method}] {Path}", context.Request.Method, context.Request.Path);
            return true;
        }

        var statusCode = ResolveStatusCode(exception);
        var isClientError = statusCode < 500;

        if (isClientError)
            _logger.LogWarning(exception,
                "[{Method}] {Path} → {StatusCode} ({ExceptionType}): {Message}",
                context.Request.Method, context.Request.Path, statusCode,
                exception.GetType().Name, exception.Message);
        else
            _logger.LogError(exception,
                "[{Method}] {Path} → {StatusCode} ({ExceptionType}): {Message}",
                context.Request.Method, context.Request.Path, statusCode,
                exception.GetType().Name, exception.Message);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Detail = isClientError ? exception.Message : "Beklenmedik bir hata oluştu.",
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

        if (exception is AppException appEx)
            problem.Extensions["errorCode"] = appEx.ErrorCode;

        if (exception is ValidationException ve)
        {
            problem.Title = "Validation Error";
            problem.Detail = "Bir veya daha fazla doğrulama hatası oluştu.";
            problem.Extensions["errors"] = ve.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }

    private static int ResolveStatusCode(Exception exception) => exception switch
    {
        AppException ae                => ae.StatusCode,
        ValidationException            => StatusCodes.Status400BadRequest,
        UnauthorizedAccessException    => StatusCodes.Status401Unauthorized,
        KeyNotFoundException            => StatusCodes.Status404NotFound,
        _                              => StatusCodes.Status500InternalServerError
    };
}
