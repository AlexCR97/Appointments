namespace Appointments.Domain.Entities;

public class Customer : Entity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsPasswordPlainText { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfileImage { get; set; }

    public Customer(Guid id, DateTime createdAt, string? createdBy, DateTime? updatedAt, string? updatedBy, DateTime? deletedAt, string? deletedBy, List<string> tags, Dictionary<string, string?> extensions) : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy, tags, extensions)
    {
    }
}
