using Appointments.Assets.Domain;
using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record FindAssetsRequest : FindRequest<Asset>
{
    public FindAssetsRequest(int PageIndex, int PageSize, string? Sort, string? Filter) : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class FindAssetsRequestHandler : IRequestHandler<FindAssetsRequest, PagedResult<Asset>>
{
    private readonly IAssetRepository _assetRepository;

    public FindAssetsRequestHandler(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<PagedResult<Asset>> Handle(FindAssetsRequest request, CancellationToken cancellationToken)
    {
        new FindRequestValidator<Asset>().ValidateAndThrow(request);

        return await _assetRepository.FindAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);
    }
}
