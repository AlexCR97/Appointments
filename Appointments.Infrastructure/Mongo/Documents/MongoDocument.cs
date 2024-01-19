using Appointments.Common.MongoClient.Abstractions;
using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Infrastructure.Mongo.Documents;

internal abstract class MongoDocument : IMongoDocument
{
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public MongoDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    protected MongoDocument(Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
    }
}
