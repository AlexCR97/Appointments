using Appointments.Common.Domain;

namespace Appointments.Assets.Domain;

public sealed class Asset : Entity
{
    public AssetPath Path { get; private set; }
    public string ContentType { get; private set; }
    public AssetTransaction Transaction { get; }

    public Asset(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        AssetPath path,
        string contentType,
        AssetTransaction transaction)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy)
    {
        Path = path;
        ContentType = contentType;
        Transaction = transaction;
    }

    public void Update(string updatedBy, AssetPath path, string contentType)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Path = path;
        ContentType = contentType;

        // TODO Add event
    }

    public static Asset Create(string createdBy, AssetPath path, string contentType)
    {
        var asset = new Asset(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            path,
            contentType,
            AssetTransaction.InProgress());

        // TODO Add event

        return asset;
    }
}

public sealed class AssetTransaction
{
    private const int _expirationSeconds = 30;

    public AssetTransactionStatus Status { get; private set; }
    public string Code { get; }
    public DateTime? ExpiresAt { get; private set; }

    public AssetTransaction(AssetTransactionStatus status, string code, DateTime? expiresAt)
    {
        Status = status;
        Code = code;
        ExpiresAt = expiresAt;
    }

    public void Complete()
    {
        Status = AssetTransactionStatus.Completed;
        ExpiresAt = null;
    }

    public void Expire()
    {
        Status = AssetTransactionStatus.Expired;
    }

    public void Fail()
    {
        Status = AssetTransactionStatus.Failed;
        ExpiresAt = DateTime.UtcNow.AddSeconds(_expirationSeconds);
    }

    public bool HasExpired()
    {
        return ExpiresAt is not null && ExpiresAt < DateTime.UtcNow;
    }

    public static AssetTransaction InProgress()
    {
        return new AssetTransaction(
            AssetTransactionStatus.InProgress,
            Guid.NewGuid().ToString(),
            DateTime.UtcNow.AddSeconds(_expirationSeconds));
    }
}

public enum AssetTransactionStatus
{
    Unknown,
    InProgress,
    Cancelled,
    Failed,
    Completed,
    Expired,
}
