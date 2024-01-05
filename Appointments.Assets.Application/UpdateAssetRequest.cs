using Appointments.Assets.Domain;
using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record UpdateAssetProfileRequest(
    string UpdatedBy,
    Guid Id,
    AssetPath Path)
    : IRequest;

internal sealed class UpdateAssetProfileRequestValidator : AbstractValidator<UpdateAssetProfileRequest>
{
    public UpdateAssetProfileRequestValidator()
    {
        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Path)
            .SetValidator(new AssetPathValidator());
    }
}

internal sealed class UpdateAssetProfileRequestHandler : IRequestHandler<UpdateAssetProfileRequest>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetStore _assetStore;
    private readonly IEventProcessor _eventProcessor;

    public UpdateAssetProfileRequestHandler(IAssetRepository assetRepository, IAssetStore assetStore, IEventProcessor eventProcessor)
    {
        _assetRepository = assetRepository;
        _assetStore = assetStore;
        _eventProcessor = eventProcessor;
    }

    public async Task Handle(UpdateAssetProfileRequest request, CancellationToken cancellationToken)
    {
        new UpdateAssetProfileRequestValidator().ValidateAndThrow(request);

        var asset = await _assetRepository.GetAsync(request.Id);

        if (await _assetStore.ExistsAsync(asset.Path.Value))
            await _assetStore.MoveAsync(asset.Path.Value, request.Path.Value);

        asset.Update(
            request.UpdatedBy,
            request.Path);

        if (asset.HasChanged)
        {
            await _assetRepository.UpdateAsync(asset);
            await _eventProcessor.ProcessAsync(asset.Events);
        }
    }
}
