namespace Appointments.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public string ResourceType { get; }

    public NotFoundException(string resourceType)
        : base("NotFound", $"Could not find {resourceType} with the specified filter.")
    {
        ResourceType = resourceType;
    }

    public NotFoundException(string resourceType, Guid id)
        : base("NotFound", $"Could not find {resourceType} with ID of {id}.")
    {
        ResourceType = resourceType;
    }
}
