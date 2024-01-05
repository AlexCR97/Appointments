using Appointments.Common.Domain.Models;
using FluentValidation;

namespace Appointments.Core.Domain.Entities;

/// <param name="CustomerId">
/// The ID of the customer if they are signed into the system.
/// If this value is null, then the customer is not signed into
/// the system and will be considered as an "anonymous" customer.
/// </param>
public sealed record AppointmentCustomer(
    Guid? CustomerId,
    string FirstName,
    string LastName,
    Email? Email,
    string? PhoneNumber,
    string? ProfileImage);

public sealed class AppointmentCustomerValidator : AbstractValidator<AppointmentCustomer>
{
    public AppointmentCustomerValidator()
    {
        When(x => x.CustomerId is not null, () =>
        {
            RuleFor(x => x.CustomerId!.Value)
                .NotEmpty();
        });

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        When(x => x.Email is not null, () =>
        {
            RuleFor(x => x.Email!.Value)
                .SetValidator(new EmailValidator());
        });

        When(x => x.PhoneNumber is not null, () =>
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty();
        });

        When(x => x.ProfileImage is not null, () =>
        {
            RuleFor(x => x.ProfileImage)
                .NotEmpty();
        });
    }
}
