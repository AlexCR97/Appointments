using Appointments.Assets.Domain;

namespace Appointments.Api.Assets.Models;

public sealed record CreateAssetRequest(
    string Path,
    string ContentType)
{
    internal Appointments.Assets.Application.CreateAssetRequest ToApplicationRequest(string createdBy)
    {
        return new Appointments.Assets.Application.CreateAssetRequest(
            createdBy,
            new AssetPath(Path),
            ContentType);
    }
}
