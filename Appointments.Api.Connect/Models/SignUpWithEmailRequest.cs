using Appointments.Common.Domain.Models;

namespace Appointments.Api.Connect.Models;

public sealed record SignUpWithEmailRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PasswordConfirm,
    string CompanyName)
{
    internal Appointments.Core.Application.Requests.Users.SignUpWithEmailRequest ToApplicationRequest()
    {
        return new Appointments.Core.Application.Requests.Users.SignUpWithEmailRequest(
            FirstName,
            LastName,
            new Email(Email),
            Password,
            PasswordConfirm,
            CompanyName);
    }
}
