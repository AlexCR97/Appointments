namespace Appointments.Domain.Models;

public class UserProfile
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfileImage { get; set; }
}
