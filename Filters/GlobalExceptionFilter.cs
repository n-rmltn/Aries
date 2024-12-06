using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Aries.Exceptions;

namespace Aries.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionFilter(
        ILogger<GlobalExceptionFilter> logger,
        IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);

        var result = new JsonResult(new {
            Title = "Error",
            Message = _env.IsDevelopment() 
                ? context.Exception.Message 
                : "An error occurred while processing your request."
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        // Custom handling for different exception types
        switch (context.Exception)
        {
            case NotFoundException:
                result.StatusCode = StatusCodes.Status404NotFound;
                break;
            case ValidationException:
                result.StatusCode = StatusCodes.Status400BadRequest;
                break;
            case UnauthorizedAccessException:
                result.StatusCode = StatusCodes.Status401Unauthorized;
                break;
        }

        context.Result = result;
        context.ExceptionHandled = true;
    }
}