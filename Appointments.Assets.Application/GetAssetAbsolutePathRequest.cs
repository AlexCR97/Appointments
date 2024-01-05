using Appointments.Assets.Domain;
using Appointments.Common.Application;
using Appointments.Common.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record GetAssetAbsolutePathRequest : GetRequest<string>
{
    public GetAssetAbsolutePathRequest(Guid Id) : base(Id)
    {
    }
}

internal sealed class GetAssetAbsolutePathRequestHandler : IRequestHandler<GetAssetAbsolutePathRequest, string>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetStore _assetStore;

    public GetAssetAbsolutePathRequestHandler(IAssetRepository assetRepository, IAssetStore assetStore)
    {
        _assetRepository = assetRepository;
        _assetStore = assetStore;
    }

    public async Task<string> Handle(GetAssetAbsolutePathRequest request, CancellationToken cancellationToken)
    {
        new GetRequestValidator<string>().ValidateAndThrow(request);

        var asset = await _assetRepository.GetAsync(request.Id);

        if (!await _assetStore.ExistsAsync(asset.Path.Value))
            throw new NotFoundException(nameof(Asset), nameof(Asset.Path), asset.Path.Value);

        return await _assetStore.GetAbsolutePathAsync(asset.Path.Value);
    }
}
