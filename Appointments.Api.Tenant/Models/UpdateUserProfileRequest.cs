namespace Appointments.Api.Tenant.Models;

public sealed record UpdateUserProfileRequest(
    string FirstName,
    string LastName)
{
    public Core.Application.Requests.Users.UpdateUserProfileRequest ToApplicationRequest(
        Guid id,
        string updatedBy)
    {
        return new Core.Application.Requests.Users.UpdateUserProfileRequest(
            id,
            updatedBy,
            FirstName,
            LastName);
    }
}
