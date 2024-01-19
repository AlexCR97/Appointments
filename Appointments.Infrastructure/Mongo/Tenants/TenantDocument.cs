using Appointments.Core.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;

namespace Appointments.Core.Infrastructure.Mongo.Tenants;

internal sealed class TenantDocument : MongoDocument
{
    public const string CollectionName = "tenants";

    public string Name { get; set; }
    public string? Slogan { get; set; }
    public string UrlId { get; set; } = null!;
    public string? Logo { get; set; }
    public SocialMediaContactDocument[] Contacts { get; set; }
    public WeeklyScheduleDocument? Schedule { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TenantDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    public TenantDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        string name, string? slogan, string urlId, string? logo, SocialMediaContactDocument[] contacts, WeeklyScheduleDocument? schedule)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        Name = name;
        Slogan = slogan;
        UrlId = urlId;
        Logo = logo;
        Contacts = contacts;
        Schedule = schedule;
    }

    internal static TenantDocument From(Tenant entity)
    {
        return new TenantDocument(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.Name,
            entity.Slogan,
            entity.UrlId.Value,
            entity.Logo,
            entity.Contacts
                .Select(SocialMediaContactDocument.From)
                .ToArray(),
            entity.Schedule is not null
                ? WeeklyScheduleDocument.From(entity.Schedule)
                : null);
    }

    internal Tenant ToEntity()
    {
        return new Tenant(
            Id,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            Name,
            Slogan,
            new TenantUrlId(UrlId),
            Logo,
            Contacts
                .Select(x => x.ToEntity())
                .ToList(),
            Schedule?.ToEntity());
    }
}
