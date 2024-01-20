using Appointments.Core.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Core.Infrastructure.Mongo.BranchOffices;

internal sealed class BranchOfficeDocument : MongoDocument
{
    public const string CollectionName = "branch-offices";

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public AddressDocument Address { get; set; }
    public SocialMediaContactDocument[] Contacts { get; set; }
    public WeeklyScheduleDocument? Schedule { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public BranchOfficeDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    public BranchOfficeDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        Guid tenantId, string name, AddressDocument address, SocialMediaContactDocument[] contacts, WeeklyScheduleDocument? schedule)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        TenantId = tenantId;
        Name = name;
        Address = address;
        Contacts = contacts;
        Schedule = schedule;
    }

    internal static BranchOfficeDocument From(BranchOffice entity)
    {
        return new BranchOfficeDocument(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.TenantId,
            entity.Name,
            AddressDocument.From(entity.Address),
            entity.Contacts
                .Select(SocialMediaContactDocument.From)
                .ToArray(),
            entity.Schedule is not null
                ? WeeklyScheduleDocument.From(entity.Schedule)
                : null);
    }

    internal BranchOffice ToEntity()
    {
        return new BranchOffice(
            Id,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            TenantId,
            Name,
            Address.ToEntity(),
            Contacts
                .Select(x => x.ToEntity())
                .ToList(),
            Schedule?.ToEntity());
    }
}
