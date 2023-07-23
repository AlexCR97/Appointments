using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Services.Events;
using Appointments.Application.Services.Files;
using Appointments.Domain.Entities;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record UploadLogoRequest(
    Guid Id,
    string FileName,
    byte[] File,
    string? UpdatedBy) : IRequest<string>;

internal sealed class UploadLogoRequestHandler : IRequestHandler<UploadLogoRequest, string>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IFileStorage _fileStorage;
    private readonly ITenantRepository _tenantRepository;

    public UploadLogoRequestHandler(IEventProcessor eventProcessor, IFileStorage fileStorage, ITenantRepository tenantRepository)
    {
        _eventProcessor = eventProcessor;
        _fileStorage = fileStorage;
        _tenantRepository = tenantRepository;
    }

    public async Task<string> Handle(UploadLogoRequest request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id);

        var imageRelativePathWithoutFileName = Path.Join("Tenants", tenant.Id.ToString());
        await _fileStorage.EnsureDirectoryAsync(imageRelativePathWithoutFileName);

        var imageName = $"Logo.{ExtractFileExtension(request.FileName)}";
        var imageRelativePathWithFileName = Path.Join("Tenants", tenant.Id.ToString(), imageName);

        await TryDeletePreviousProfileImageAsync(tenant);

        await _fileStorage.WriteAsync(imageRelativePathWithFileName, request.File);

        tenant.UpdateLogo(
            request.UpdatedBy,
            imageRelativePathWithFileName);

        if (tenant.HasChanged)
        {
            await _tenantRepository.UpdateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }

        return imageRelativePathWithFileName;
    }

    private static string ExtractFileExtension(string fileName)
    {
        return fileName
            .Split('.')
            .Last();
    }

    private async Task TryDeletePreviousProfileImageAsync(Tenant tenant)
    {
        if (string.IsNullOrWhiteSpace(tenant.Logo))
            return;

        if (await _fileStorage.ExistsAsync(tenant.Logo))
            await _fileStorage.DeleteAsync(tenant.Logo);
    }
}
