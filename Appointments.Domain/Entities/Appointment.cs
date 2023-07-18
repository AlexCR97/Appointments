namespace Appointments.Domain.Entities;

public class Appointment : Entity
{
    public Guid TenantId { get; set; }

    /// <summary>
    /// The branch office where the appointment will take place.
    /// </summary>
    public Guid BranchOfficeId { get; set; }

    /// <summary>
    /// The service that the appointment is for.
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// The ID of the user that will provider the service. If this value is null,
    /// then it is up to the company to decide who will attend the customer.
    /// </summary>
    public Guid? ServiceProviderId { get; set; }

    /// <summary>
    /// The date in which the appointment will take place.
    /// </summary>
    public DateTime AppointmentDate { get; set; }

    /// <summary>
    /// A user-friendly code that can be used to uniquely identify the appointment.
    /// </summary>
    public string AppointmentCode { get; set; }

    /// <summary>
    /// The customer who made the appointment.
    /// </summary>
    public AppointmentCustomer Customer { get; set; }

    public string? Notes { get; set; }
}
