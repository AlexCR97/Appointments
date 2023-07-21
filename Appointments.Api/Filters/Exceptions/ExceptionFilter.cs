using Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Appointments.Api.Filters.Exceptions;

internal class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    private readonly IProblemDetailsFactory<Exception> _problemDetailsFactory;

    public ExceptionFilter(ILogger<ExceptionFilter> logger, IProblemDetailsFactory<Exception> problemDetailsFactory)
    {
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Error while processing the request: {Method} {Path}",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path);

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(context.Exception);

        context.Result = new JsonResult(problemDetails)
        {
            StatusCode = problemDetails.Status,
        };

        context.ExceptionHandled = true;
    }
}
