using Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Appointments.Api.Filters.Exceptions;

internal class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    private readonly IProblemDetailsFactory<Exception> _problemDetailsFactory;

    private readonly IReadOnlyList<Type> _ignoredExceptionsForLogs = new List<Type>
    {
        typeof(ValidationException),
    };

    public ExceptionFilter(ILogger<ExceptionFilter> logger, IProblemDetailsFactory<Exception> problemDetailsFactory)
    {
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public void OnException(ExceptionContext context)
    {
        var isIgnoredException = _ignoredExceptionsForLogs.Contains(context.Exception.GetType());
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(context.Exception);

        if (!isIgnoredException)
        {
            _logger.LogError(context.Exception, "Error while processing the request: {RequestMethod} {RequestPath}. Problem details: {@ProblemDetails}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                problemDetails);
        }

        context.Result = new JsonResult(problemDetails)
        {
            StatusCode = problemDetails.Status,
        };

        context.ExceptionHandled = true;
    }
}
