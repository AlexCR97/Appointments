using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record GetAssetContentsRequest(
    Guid Id)
    : IRequest<byte[]>;

internal sealed class GetAssetContentsRequestValidator : AbstractValidator<GetAssetContentsRequest>
{
    public GetAssetContentsRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal sealed class GetAssetContentsRequestHandler : IRequestHandler<GetAssetContentsRequest, byte[]>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetStore _assetStore;

    public GetAssetContentsRequestHandler(IAssetRepository assetRepository, IAssetStore assetStore)
    {
        _assetRepository = assetRepository;
        _assetStore = assetStore;
    }

    public async Task<byte[]> Handle(GetAssetContentsRequest request, CancellationToken cancellationToken)
    {
        new GetAssetContentsRequestValidator().ValidateAndThrow(request);

        var asset = await _assetRepository.GetAsync(request.Id);

        return await _assetStore.ReadAsync(asset.Path.Value, cancellationToken);
    }
}
