using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TDM.Server.Application.Exceptions;

namespace TDM.Server.Middleware.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                _logger.LogInformation("Запрос был отменен: {RequestPath}", context.Request.Path);
            }
            else
            {
                _logger.LogError(ex, "Произошло необработанное исключение для {RequestPath}: {Message}",
                    context.Request.Path, ex.Message);
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

        object response = new { error = "Произошла внутренняя ошибка сервера. Пожалуйста, обратитесь к администратору." };

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                response = validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                break;

            case OperationCanceledException:
                statusCode = HttpStatusCode.BadRequest;
                response = new { error = "Операция была отменена" };
                break;

            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                response = new { error = badRequestException.Message };
                break;

            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                response = new { error = notFoundException.Message };
                break;

            case ConflictException conflictException:
                statusCode = HttpStatusCode.Conflict;
                response = new { error = conflictException.Message };
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
