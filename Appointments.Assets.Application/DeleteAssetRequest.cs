using Appointments.Common.Application;
using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record DeleteAssetRequest : DeleteRequest
{
    public DeleteAssetRequest(string DeletedBy, Guid Id) : base(DeletedBy, Id)
    {
    }
}

internal sealed class DeleteAssetRequestHandler : IRequestHandler<DeleteAssetRequest>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetStore _assetStore;
    private readonly IEventProcessor _eventProcessor;

    public DeleteAssetRequestHandler(IAssetRepository assetRepository, IAssetStore assetStore, IEventProcessor eventProcessor)
    {
        _assetRepository = assetRepository;
        _assetStore = assetStore;
        _eventProcessor = eventProcessor;
    }

    public async Task Handle(DeleteAssetRequest request, CancellationToken cancellationToken)
    {
        new DeleteRequestValidator().ValidateAndThrow(request);

        var asset = await _assetRepository.GetAsync(request.Id);

        if (await _assetStore.ExistsAsync(asset.Path.Value))
            await _assetStore.DeleteAsync(asset.Path.Value);
        
        await _assetRepository.DeleteAsync(asset.Id);
        await _eventProcessor.ProcessAsync(asset.Events);
    }
}
