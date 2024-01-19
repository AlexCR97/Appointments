namespace Appointments.Common.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string resourceType, string identifierType, string identifierValue)
        : base($"NotFound", $"Could not find {resourceType} with {identifierType}={identifierValue}")
    {
    }
}
