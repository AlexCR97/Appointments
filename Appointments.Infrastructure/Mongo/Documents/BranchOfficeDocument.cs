using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Infrastructure.Mongo.Documents;

internal class BranchOfficeDocument : MongoDocument
{
    public const string CollectionName = "branch-offices";

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;
    public LocationDocument Location { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<SocialMediaContactDocument> SocialMediaContacts { get; set; } = null!;
    public WeeklyScheduleDocument WeeklySchedule { get; set; } = null!;
}
