using Appointments.Assets.Domain;
using Appointments.Common.Application;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record GetAssetRequest : GetRequest<Asset>
{
    public GetAssetRequest(Guid Id) : base(Id)
    {
    }
}

internal sealed class GetAssetRequestHandler : IRequestHandler<GetAssetRequest, Asset>
{
    private readonly IAssetRepository _assetRepository;

    public GetAssetRequestHandler(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<Asset> Handle(GetAssetRequest request, CancellationToken cancellationToken)
    {
        new GetRequestValidator<Asset>().ValidateAndThrow(request);

        return await _assetRepository.GetAsync(request.Id);
    }
}
