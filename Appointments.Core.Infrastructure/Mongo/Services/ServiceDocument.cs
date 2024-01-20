using Appointments.Core.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Core.Infrastructure.Mongo.Services;

internal sealed class ServiceDocument : MongoDocument
{
    public const string CollectionName = "services";

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public IndexedResourceDocument[] Images { get; set; }
    public string[] TermsAndConditions { get; set; }
    public string? Notes { get; set; }
    public TimeSpan? CustomerDuration { get; set; }
    public TimeSpan? CalendarDuration { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ServiceDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public ServiceDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        Guid tenantId, string name, string? description, decimal price, IndexedResourceDocument[] images, string[] termsAndConditions, string? notes, TimeSpan? customerDuration, TimeSpan? calendarDuration)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        Price = price;
        Images = images;
        TermsAndConditions = termsAndConditions;
        Notes = notes;
        CustomerDuration = customerDuration;
        CalendarDuration = calendarDuration;
    }

    internal static ServiceDocument From(Service entity)
    {
        return new ServiceDocument(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.TenantId,
            entity.Name,
            entity.Description,
            entity.Price,
            entity.Images
                .Select(IndexedResourceDocument.From)
                .ToArray(),
            entity.TermsAndConditions.ToArray(),
            entity.Notes,
            entity.CustomerDuration,
            entity.CalendarDuration);
    }

    internal Service ToEntity()
    {
        return new Service(
            Id,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            TenantId,
            Name,
            Description,
            Price,
            Images
                .Select(x => x.ToModel())
                .ToList(),
            TermsAndConditions.ToList(),
            Notes,
            CustomerDuration,
            CalendarDuration);
    }
}
