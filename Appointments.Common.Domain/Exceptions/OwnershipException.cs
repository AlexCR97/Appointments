namespace Appointments.Common.Domain.Exceptions;

public class OwnershipException : DomainException
{
    public OwnershipException(string owner, string ownerIdentifier, string resource, string resourceIdentifier)
        : base($"InvalidOwnership", $"The resource {owner}={ownerIdentifier} does not have a resource {resource}={resourceIdentifier}")
    {
    }
}
