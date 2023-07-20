namespace Appointments.Infrastructure.Mongo.Documents;

internal class TenantDocument : MongoDocument
{
    public const string CollectionName = "tenants";

    public string Name { get; set; } = null!;
    public string? Slogan { get; set; }
    public string UrlId { get; set; } = null!;
    public string? Logo { get; set; }
    public List<SocialMediaContactDocument> SocialMediaContacts { get; set; } = null!;
    public WeeklyScheduleDocument? WeeklySchedule { get; set; }
}
