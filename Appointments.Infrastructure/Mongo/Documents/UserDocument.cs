namespace Appointments.Infrastructure.Mongo.Documents;

internal class UserDocument : MongoDocument
{
    public const string CollectionName = "users";

    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsPasswordPlainText { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfileImage { get; set; }
}
