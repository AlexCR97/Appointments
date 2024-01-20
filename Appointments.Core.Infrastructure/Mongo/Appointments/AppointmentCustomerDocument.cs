using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Core.Infrastructure.Mongo.Appointments;

internal sealed record AppointmentCustomerDocument
{
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid? CustomerId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? ProfileImage { get; init; }

    public AppointmentCustomerDocument(Guid? customerId, string firstName, string lastName, string? email, string? phoneNumber, string? profileImage)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        ProfileImage = profileImage;
    }

    internal static AppointmentCustomerDocument From(AppointmentCustomer customer)
    {
        return new AppointmentCustomerDocument(
            customer.CustomerId,
            customer.FirstName,
            customer.LastName,
            customer.Email?.Value,
            customer.PhoneNumber,
            customer.ProfileImage);
    }

    internal AppointmentCustomer ToEntity()
    {
        return new AppointmentCustomer(
            CustomerId,
            FirstName,
            LastName,
            Email is not null
                ? new Email(Email)
                : null,
            PhoneNumber,
            ProfileImage);
    }
}
