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
    public Core.Application.Requests.Users.SignUpWithEmailRequest ToApplicationRequest()
    {
        return new Core.Application.Requests.Users.SignUpWithEmailRequest(
            FirstName,
            LastName,
            new Email(Email),
            Password,
            PasswordConfirm,
            CompanyName);
    }
}
