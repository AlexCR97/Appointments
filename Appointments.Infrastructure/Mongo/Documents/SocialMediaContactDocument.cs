namespace Appointments.Infrastructure.Mongo.Documents;

internal class SocialMediaContactDocument
{
    public string Contact { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? OtherType { get; set; }
}
