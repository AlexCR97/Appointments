using Appointments.Assets.Domain;

namespace Appointments.Api.Assets.Models;

public sealed record AssetResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    string Path,
    string ContentType,
    AssetTransactionResponse Transaction)
{
    internal static AssetResponse From(Asset entity)
    {
        return new AssetResponse(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.Path.Value,
            entity.ContentType,
            AssetTransactionResponse.From(entity.Transaction));
    }
}

public sealed record AssetTransactionResponse(
    string Status,
    string Code,
    DateTime? ExpiresAt)
{
    internal static AssetTransactionResponse From(AssetTransaction entity)
    {
        return new AssetTransactionResponse(
            entity.Status.ToString(),
            entity.Code,
            entity.ExpiresAt);
    }
}
