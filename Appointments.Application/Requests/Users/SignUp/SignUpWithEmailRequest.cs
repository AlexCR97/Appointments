using MediatR;

namespace Appointments.Application.Requests.Users.SignUp;

public class SignUpWithEmailRequest : IRequest<Guid>
{
    public string TenantName { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Password { get; }

    public SignUpWithEmailRequest(string tenantName, string firstName, string lastName, string email, string password)
    {
        TenantName = tenantName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }
}
