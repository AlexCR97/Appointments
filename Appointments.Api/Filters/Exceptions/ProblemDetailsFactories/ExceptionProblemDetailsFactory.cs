﻿using Appointments.Common.Domain.Exceptions;
using Appointments.Core.Infrastructure.DependencyInjection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;

internal class ExceptionProblemDetailsFactory : IProblemDetailsFactory<Exception>
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExceptionProblemDetailsFactory(IConfiguration configuration, IHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    private bool IsEnvironmentDevelopment
        => _hostEnvironment.EnvironmentName == "Development";

    private string ErrorDocsUrl
        => _configuration.GetRequiredString("Errors:DocsUrl");

    public ProblemDetails CreateProblemDetails(Exception exception)
    {
        var builder = new ProblemDetailsBuilder()
            .WithType(exception.HelpLink ?? $"{ErrorDocsUrl}#InternalServerError")
            .WithTitle("InternalServerError")
            .WithStatus(StatusCodes.Status500InternalServerError)
            .WithDetail(exception.GetMessages());

        if (_httpContextAccessor.HttpContext is not null)
            builder.WithInstance(_httpContextAccessor.HttpContext.Request.Path);

        if (IsEnvironmentDevelopment)
            builder.IncludeStackTraces(exception);

        if (exception is ValidationException validationException)
            OverrideWith(builder, validationException);

        if (exception is DomainException domainException)
            OverrideWith(builder, domainException);

        return builder.Build();
    }

    private void OverrideWith(ProblemDetailsBuilder builder, ValidationException exception)
    {
        builder
            .WithType($"{ErrorDocsUrl}#InvalidModel")
            .WithTitle("InvalidModel")
            .WithStatus(StatusCodes.Status400BadRequest)
            .WithExtension("errors", exception.Errors.Select(error => new
            {
                propertyName = error.PropertyName,
                errorCode = error.ErrorCode,
                errorMessage = error.ErrorMessage,
                attemptedValue = error.AttemptedValue,
            }));
    }

    private void OverrideWith(ProblemDetailsBuilder builder, DomainException exception)
    {
        builder
            .WithType($"{ErrorDocsUrl}#{exception.Code}")
            .WithTitle(exception.Code)
            .WithStatus(StatusCodes.Status400BadRequest);

        if (exception is NotFoundException notFoundException)
            OverrideWith(builder, notFoundException);
    }

    private void OverrideWith(ProblemDetailsBuilder builder, NotFoundException exception)
    {
        builder
            .WithStatus(StatusCodes.Status404NotFound);
    }
}

internal static class ExceptionProblemDetailsFactoryExtensions
{
    public static void IncludeStackTraces(this ProblemDetailsBuilder builder, Exception exception)
    {
        builder.WithExtension("stackTrace", exception.StackTrace);

        if (exception.InnerException?.StackTrace is not null)
            builder.WithExtension("innerStackTrace", exception.InnerException.StackTrace);
    }
}
