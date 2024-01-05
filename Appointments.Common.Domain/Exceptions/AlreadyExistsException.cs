namespace Appointments.Common.Domain.Exceptions;

public class AlreadyExistsException : DomainException
{
    public AlreadyExistsException(string resourceType, string identifierType, string identifierValue)
        : base($"AlreadyExists", $"A resource of type {resourceType} with {identifierType}={identifierValue} already exists")
    {
    }
}
