using Appointments.Application.Requests.Users.SignUp;
using FluentValidation;

namespace Appointments.Application.Validations.Users;

internal class SignUpWithEmailRequestValidator : AbstractValidator<SignUpWithEmailRequest>
{
    public SignUpWithEmailRequestValidator()
    {
        RuleFor(x => x.TenantName)
            .NotEmpty()
            .MaxNameLength();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaxNameLength();

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaxNameLength();

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Password)
            .Password();
    }
}
