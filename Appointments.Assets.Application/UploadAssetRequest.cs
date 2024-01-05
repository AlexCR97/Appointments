using Appointments.Assets.Domain;
using Appointments.Common.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record UploadAssetProfileRequest(
    AssetPath Path,
    byte[] Data)
    : IRequest;

internal sealed class UploadAssetProfileRequestValidator : AbstractValidator<UploadAssetProfileRequest>
{
    public UploadAssetProfileRequestValidator()
    {
        RuleFor(x => x.Path)
            .SetValidator(new AssetPathValidator());
    }
}

internal sealed class UploadAssetProfileRequestHandler : IRequestHandler<UploadAssetProfileRequest>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetStore _assetStore;

    public UploadAssetProfileRequestHandler(IAssetRepository assetRepository, IAssetStore assetStore)
    {
        _assetRepository = assetRepository;
        _assetStore = assetStore;
    }

    public async Task Handle(UploadAssetProfileRequest request, CancellationToken cancellationToken)
    {
        new UploadAssetProfileRequestValidator().ValidateAndThrow(request);

        if (await _assetStore.ExistsAsync(request.Path.Value))
            throw new AlreadyExistsException(nameof(Asset), nameof(Asset.Path), request.Path.Value);
        else
            await _assetStore.WriteAsync(request.Path.Value, request.Data);
    }
}
