using Appointments.Assets.Domain;
using Appointments.Common.Domain;
using Appointments.Common.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Appointments.Assets.Application;

public sealed record CreateAssetRequest(
    string CreatedBy,
    AssetPath Path,
    string ContentType)
    : IRequest<AssetCreatedResult>;

public sealed record AssetCreatedResult(
    Guid Id,
    string TransactionCode);

internal sealed class CreateAssetRequestValidator : AbstractValidator<CreateAssetRequest>
{
    public CreateAssetRequestValidator()
    {
        RuleFor(x => x.CreatedBy)
            .NotEmpty();

        RuleFor(x => x.Path)
            .SetValidator(new AssetPathValidator());

        RuleFor(x => x.ContentType)
            .NotEmpty();
    }
}

internal sealed class CreateAssetRequestHandler : IRequestHandler<CreateAssetRequest, AssetCreatedResult>
{
    private readonly IAssetRepository _assetRepository;
    private readonly IEventProcessor _eventProcessor;

    public CreateAssetRequestHandler(IAssetRepository assetRepository, IEventProcessor eventProcessor)
    {
        _assetRepository = assetRepository;
        _eventProcessor = eventProcessor;
    }

    public async Task<AssetCreatedResult> Handle(CreateAssetRequest request, CancellationToken cancellationToken)
    {
        new CreateAssetRequestValidator().ValidateAndThrow(request);

        var existingAsset = await _assetRepository.GetByPathOrDefaultAsync(request.Path);

        if (existingAsset is not null)
            throw new AlreadyExistsException(nameof(Asset), nameof(Asset.Path), request.Path.Value);

        var asset = Asset.Create(
            request.CreatedBy,
            request.Path,
            request.ContentType);

        if (asset.HasChanged)
        {
            await _assetRepository.CreateAsync(asset);
            await _eventProcessor.ProcessAsync(asset.Events);
        }

        return new AssetCreatedResult(
            asset.Id,
            asset.Transaction.Code);
    }
}
