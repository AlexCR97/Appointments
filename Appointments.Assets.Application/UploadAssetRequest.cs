using Appointments.Assets.Domain;
using Appointments.Common.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record UploadAssetRequest(
    AssetPath Path,
    byte[] Data,
    string TransactionCode)
    : IRequest<AssetTransactionStatus>;

internal sealed class UploadAssetRequestValidator : AbstractValidator<UploadAssetRequest>
{
    public UploadAssetRequestValidator()
    {
        RuleFor(x => x.Path)
            .SetValidator(new AssetPathValidator());

        RuleFor(x => x.Data)
            .NotEmpty();

        RuleFor(x => x.TransactionCode)
            .NotEmpty();
    }
}

internal sealed class UploadAssetRequestHandler : IRequestHandler<UploadAssetRequest, AssetTransactionStatus>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetStore _assetStore;

    public UploadAssetRequestHandler(IAssetRepository assetRepository, IAssetStore assetStore)
    {
        _assetRepository = assetRepository;
        _assetStore = assetStore;
    }

    public async Task<AssetTransactionStatus> Handle(UploadAssetRequest request, CancellationToken cancellationToken)
    {
        new UploadAssetRequestValidator().ValidateAndThrow(request);

        var asset = await _assetRepository.GetByTransactionCode(request.TransactionCode);

        if (request.Path.Value != asset.Path.Value)
            throw new OwnershipException(nameof(Asset), asset.Id.ToString(), nameof(Asset.Path), request.Path.Value);

        if (asset.Transaction.Status == AssetTransactionStatus.Unknown)
            return AssetTransactionStatus.Unknown;

        if (asset.Transaction.Status == AssetTransactionStatus.InProgress)
            return await TryUploadAssetAsync(request, asset);

        if (asset.Transaction.Status == AssetTransactionStatus.Cancelled)
            return await TryUploadAssetAsync(request, asset);

        if (asset.Transaction.Status == AssetTransactionStatus.Failed)
            return await TryUploadAssetAsync(request, asset);

        if (asset.Transaction.Status == AssetTransactionStatus.Completed)
            return AssetTransactionStatus.Completed;

        if (asset.Transaction.Status == AssetTransactionStatus.Expired)
            return AssetTransactionStatus.Expired;

        throw new InvalidOperationException(@$"No such scenario for {nameof(AssetTransactionStatus)}=""{asset.Transaction.Status}""");
    }

    private async Task<AssetTransactionStatus> TryUploadAssetAsync(UploadAssetRequest request, Asset asset)
    {
        if (asset.Transaction.HasExpired())
        {
            asset.Transaction.Expire();
            await _assetRepository.UpdateAsync(asset);
            return AssetTransactionStatus.Expired;
        }

        try
        {
            await _assetStore.WriteAsync(request.Path.Value, request.Data);
            asset.Transaction.Complete();
            await _assetRepository.UpdateAsync(asset);
            return AssetTransactionStatus.Completed;
        }
        catch
        {
            asset.Transaction.Fail();
            await _assetRepository.UpdateAsync(asset);
            return AssetTransactionStatus.Failed;
        }
    }
}
