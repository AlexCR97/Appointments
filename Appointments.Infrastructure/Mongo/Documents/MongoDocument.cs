using Appointments.Common.MongoClient.Abstractions;
using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Infrastructure.Mongo.Documents;

internal class MongoDocument : IMongoDocument
{
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public List<string> Tags { get; set; } = new();
    public Dictionary<string, string?> Extensions { get; set; } = new();
}
