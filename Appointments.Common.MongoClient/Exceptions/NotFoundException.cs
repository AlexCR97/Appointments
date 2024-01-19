namespace Appointments.Common.MongoClient.Exceptions;

public class NotFoundException<TDocument> : MongoException
{
    public NotFoundException()
        : base("NotFound", $"Could not find {typeof(TDocument).Name} with the specified filter.")
    {
    }

    public NotFoundException(Guid id)
        : base("NotFound", $"Could not find {typeof(TDocument).Name} with ID of {id}.")
    {
    }
}
