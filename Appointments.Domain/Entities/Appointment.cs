namespace Appointments.Domain.Entities;

public sealed class Appointment : Entity
{
    /// <summary>
    /// The ID of the tenant that the appointment belongs to.
    /// </summary>
    public Guid TenantId { get; }

    /// <summary>
    /// The ID of the branch office where the appointment will take place.
    /// </summary>
    public Guid BranchOfficeId { get; }

    /// <summary>
    /// The ID of the service that the appointment is for.
    /// </summary>
    public Guid ServiceId { get; }

    /// <summary>
    /// The ID of the user that will provide the service. If this value is null,
    /// then it is up to the company to decide who will attend the customer.
    /// </summary>
    public Guid? ServiceProviderId { get; private set; }

    /// <summary>
    /// The date in which the appointment will take place.
    /// </summary>
    public DateTime AppointmentDate { get; }

    /// <summary>
    /// A user friendly code that the customer and tenant can use to identify the appointment.
    /// </summary>
    public string AppointmentCode { get; }

    /// <summary>
    /// A snapshot of the customer who made the appointment.
    /// </summary>
    public AppointmentCustomer Customer { get; }

    /// <summary>
    /// Any notes or additional information about the appointment.
    /// </summary>
    public string? Notes { get; private set; }

    public AppointmentStatus Status { get; private set; }

    public Appointment(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        Guid tenantId, Guid branchOfficeId, Guid serviceId, Guid? serviceProviderId, DateTime appointmentDate, string appointmentCode, AppointmentCustomer customer, string? notes, AppointmentStatus status)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy)
    {
        TenantId = tenantId;
        BranchOfficeId = branchOfficeId;
        ServiceId = serviceId;
        ServiceProviderId = serviceProviderId;
        AppointmentDate = appointmentDate;
        AppointmentCode = appointmentCode;
        Customer = customer;
        Notes = notes;
        Status = status;
    }

    public void SetStatus(string updatedBy, AppointmentStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        
        // TODO Add event
    }

    public void Update(
        string updatedBy,
        Guid? serviceProviderId,
        string? notes)
    {
        ServiceProviderId = serviceProviderId;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        // TODO Add event
    }
}

/// <param name="CustomerId">
/// The ID of the customer if they are signed into the system.
/// If this value is null, then the customer is not signed into
/// the system and will be considered as an "anonymous" customer.
/// </param>
public record AppointmentCustomer(
    Guid? CustomerId,
    string FirstName,
    string LastName,
    string? Email,
    string? PhoneNumber,
    string? ProfileImage);

public enum AppointmentStatus
{
    /// <summary>
    /// The appointment is queued for review.
    /// </summary>
    Queued,

    /// <summary>
    /// The appointment is under review.
    /// </summary>
    UnderReview,

    /// <summary>
    /// The appointment was rejected.
    /// </summary>
    Rejected,

    /// <summary>
    /// The appointment is approved for schedule.
    /// </summary>
    Approved,

    /// <summary>
    /// The appointment was cancelled by the tenant.
    /// </summary>
    CancelledByTenant,

    /// <summary>
    /// The appointment was cancelled by the customer.
    /// </summary>
    CancelledByCustomer,

    /// <summary>
    /// The customer did not show up to the appointment.
    /// </summary>
    NoShow,

    /// <summary>
    /// The appointment is currently taking place.
    /// </summary>
    InProgress,

    /// <summary>
    /// The appointment has been completed successfully.
    /// </summary>
    Completed,
}
