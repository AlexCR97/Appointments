using Appointments.Common.Domain;

namespace Appointments.Assets.Domain;

public sealed class Asset : Entity
{
    public AssetPath Path { get; private set; }

    public Asset(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        AssetPath path)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy)
    {
        Path = path;
    }

    public void Update(string updatedBy, AssetPath path)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Path = path;

        // TODO Add event
    }

    public static Asset Create(string createdBy, AssetPath path)
    {
        var asset = new Asset(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            path);

        // TODO Add event

        return asset;
    }
}
