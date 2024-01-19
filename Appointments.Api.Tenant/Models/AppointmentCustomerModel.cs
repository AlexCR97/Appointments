using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record AppointmentCustomerModel(
    Guid? CustomerId,
    string FirstName,
    string LastName,
    string? Email,
    string? PhoneNumber,
    string? ProfileImage)
{
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

    internal static AppointmentCustomerModel From(AppointmentCustomer customer)
    {
        return new AppointmentCustomerModel(
            customer.CustomerId,
            customer.FirstName,
            customer.LastName,
            customer.Email?.Value,
            customer.PhoneNumber,
            customer.ProfileImage);
    }
}
