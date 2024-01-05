using Appointments.Core.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Core.Infrastructure.Mongo.Appointments;

internal sealed class AppointmentDocument : MongoDocument
{
    public const string CollectionName = "appointments";

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid TenantId { get; set; }

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid BranchOfficeId { get; set; }

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid ServiceId { get; set; }

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid? ServiceProviderId { get; set; }

    public DateTime AppointmentDate { get; set; }
    public string AppointmentCode { get; set; }
    public AppointmentCustomerDocument Customer { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public AppointmentDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    public AppointmentDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy, Guid tenantId, Guid branchOfficeId, Guid serviceId, Guid? serviceProviderId, DateTime appointmentDate, string appointmentCode, AppointmentCustomerDocument customer, string? notes, string status)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
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

    internal static AppointmentDocument From(Appointment entity)
    {
        return new AppointmentDocument(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.TenantId,
            entity.BranchOfficeId,
            entity.ServiceId,
            entity.ServiceProviderId,
            entity.AppointmentDate,
            entity.AppointmentCode.Value,
            AppointmentCustomerDocument.From(entity.Customer),
            entity.Notes,
            entity.Status.ToString());
    }

    internal Appointment ToEntity()
    {
        return new Appointment(
            Id,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            TenantId,
            BranchOfficeId,
            ServiceId,
            ServiceProviderId,
            AppointmentDate,
            new AppointmentCode(AppointmentCode),
            Customer.ToEntity(),
            Notes,
            Enum.Parse<AppointmentStatus>(Status));
    }
}
