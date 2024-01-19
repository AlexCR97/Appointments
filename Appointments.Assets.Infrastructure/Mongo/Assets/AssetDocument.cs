using Appointments.Assets.Domain;
using Appointments.Assets.Infrastructure.Mongo.Documents;

namespace Appointments.Assets.Infrastructure.Mongo.Assets;

internal sealed class AssetDocument : MongoDocument
{
    public const string CollectionName = "assets";

    public string Path { get; set; }
    public string ContentType { get; set; }
    public AssetTransactionDocument Transaction { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public AssetDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    public AssetDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        string path, string contentType, AssetTransactionDocument transaction)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        Path = path;
        ContentType = contentType;
        Transaction = transaction;
    }

    internal static AssetDocument From(Asset entity)
    {
        return new AssetDocument(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.Path.Value,
            entity.ContentType,
            AssetTransactionDocument.From(entity.Transaction));
    }

    internal Asset ToEntity()
    {
        return new Asset(
            Id,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            new AssetPath(Path),
            ContentType,
            Transaction.ToEntity());
    }
}

internal sealed record AssetTransactionDocument(
    string Status,
    string Code,
    DateTime? ExpiresAt)
{
    internal AssetTransaction ToEntity()
    {
        return new AssetTransaction(
            Enum.Parse<AssetTransactionStatus>(Status),
            Code,
            ExpiresAt);
    }

    internal static AssetTransactionDocument From(AssetTransaction entity)
    {
        return new AssetTransactionDocument(
            entity.Status.ToString(),
            entity.Code,
            entity.ExpiresAt);
    }
}
