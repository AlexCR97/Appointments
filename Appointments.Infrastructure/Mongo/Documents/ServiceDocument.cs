using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Infrastructure.Mongo.Documents;

internal class ServiceDocument : MongoDocument
{
    public const string CollectionName = "services";

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public TimeSpan? CustomerDuration { get; set; }
    public TimeSpan? CalendarDuration { get; set; }
    public List<IndexedImageDocument> Images { get; set; } = null!;
    public List<string> TermsAndConditions { get; set; } = null!;
}
