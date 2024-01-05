using Appointments.Common.Domain.Exceptions;

namespace Appointments.Api.Tenant.Exceptions;

internal class IdMismatchException : DomainException
{
    public IdMismatchException()
        : base("IdMismatch", "The ID in the request payload must match the ID in the URL")
    {
    }

    public static void ThrowIfMismatch(string urlId, string payloadId)
    {
        if (payloadId != urlId)
            throw new IdMismatchException();
    }
}
