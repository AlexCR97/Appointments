using Appointments.Assets.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record GetAssetByPathRequest(
    AssetPath Path)
    : IRequest<Asset>;

internal sealed class GetAssetByPathRequestValidator : AbstractValidator<GetAssetByPathRequest>
{
    public GetAssetByPathRequestValidator()
    {
        RuleFor(x => x.Path)
            .SetValidator(new AssetPathValidator());
    }
}

internal sealed class GetAssetByPathRequestHandler : IRequestHandler<GetAssetByPathRequest, Asset>
{
    private readonly IAssetRepository _assetRepository;

    public GetAssetByPathRequestHandler(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<Asset> Handle(GetAssetByPathRequest request, CancellationToken cancellationToken)
    {
        new GetAssetByPathRequestValidator().ValidateAndThrow(request);

        return await _assetRepository.GetByPathAsync(request.Path);
    }
}
