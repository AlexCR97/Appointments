namespace Appointments.Domain.Entities;

public class AppointmentCustomer
{
    /// <summary>
    /// The ID of the customer if they are signed into the system.
    /// If this value is null, then the customer is not signed into
    /// the system and will be considered as an "anonymous" customer.
    /// </summary>
    public Guid? CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? ProfileImage { get; set; }
}
