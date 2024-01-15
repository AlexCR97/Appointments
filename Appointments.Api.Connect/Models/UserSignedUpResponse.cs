using Appointments.Core.Application.Requests.Users;

namespace Appointments.Api.Connect.Models;

public sealed record UserSignedUpResponse(
    Guid UserId,
    Guid TenantId)
{
    internal static UserSignedUpResponse From(UserSignedUpResult result)
    {
        return new UserSignedUpResponse(
            result.UserId,
            result.TenantId);
    }
}