using Appointments.Jobs.Infrastructure.Mongo.Documents;

namespace Appointments.Jobs.Infrastructure.Mongo.Jobs;

internal class JobDocument : MongoDocument
{
    public const string CollectionName = "jobs-jobs";
}
