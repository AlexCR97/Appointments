using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;

internal interface IProblemDetailsFactory<TException>
    where TException : Exception
{
    ProblemDetails CreateProblemDetails(TException exception);
}
