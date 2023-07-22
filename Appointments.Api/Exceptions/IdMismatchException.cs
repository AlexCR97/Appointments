namespace Appointments.Api.Exceptions;

internal class IdMismatchException : Application.Exceptions.ApplicationException
{
    public IdMismatchException()
        : base("IdMismatch", $"The ID in the URL does not match the ID in the request body.")
    {
    }

    public static void ThrowIfMismatch(Guid urlId, Guid requestId)
    {
        if (urlId != requestId)
            throw new IdMismatchException();
    }
}
