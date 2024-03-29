﻿using Appointments.Common.Domain;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Tenants;

public sealed record UploadTenantLogoRequest(
    Guid Id,
    string UpdatedBy,
    string FileName,
    byte[] File)
    : IRequest<string>;

internal sealed class UploadTenantLogoRequestValidator : AbstractValidator<UploadTenantLogoRequest>
{
    public UploadTenantLogoRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.FileName)
            .NotEmpty();

        RuleFor(x => x.File)
            .NotEmpty();
    }
}

internal sealed class UploadTenantLogoRequestHandler : IRequestHandler<UploadTenantLogoRequest, string>
{
    //private readonly IEventProcessor _eventProcessor;
    //private readonly IFileStorage _fileStorage;
    //private readonly ITenantRepository _tenantRepository;

    //public UploadTenantLogoRequestHandler(IEventProcessor eventProcessor, IFileStorage fileStorage, ITenantRepository tenantRepository)
    //{
    //    _eventProcessor = eventProcessor;
    //    _fileStorage = fileStorage;
    //    _tenantRepository = tenantRepository;
    //}

    public async Task<string> Handle(UploadTenantLogoRequest request, CancellationToken cancellationToken)
    {
        // TODO Implement
        throw new NotImplementedException();

        //var tenant = await _tenantRepository.GetAsync(request.Id);

        //var imageRelativePathWithoutFileName = Path.Join("Tenants", tenant.Id.ToString());
        //await _fileStorage.EnsureDirectoryAsync(imageRelativePathWithoutFileName);

        //var imageName = $"Logo.{ExtractFileExtension(request.FileName)}";
        //var imageRelativePathWithFileName = Path.Join("Tenants", tenant.Id.ToString(), imageName);

        //await TryDeletePreviousProfileImageAsync(tenant);

        //await _fileStorage.WriteAsync(imageRelativePathWithFileName, request.File);

        //tenant.SetLogo(
        //    request.UpdatedBy,
        //    imageRelativePathWithFileName);

        //if (tenant.HasChanged)
        //{
        //    await _tenantRepository.UpdateAsync(tenant);
        //    await _eventProcessor.ProcessAsync(tenant.Events);
        //}

        //return imageRelativePathWithFileName;
    }

    //private static string ExtractFileExtension(string fileName)
    //{
    //    return fileName
    //        .Split('.')
    //        .Last();
    //}

    //private async Task TryDeletePreviousProfileImageAsync(Tenant tenant)
    //{
    //    if (string.IsNullOrWhiteSpace(tenant.Logo))
    //        return;

    //    if (await _fileStorage.ExistsAsync(tenant.Logo))
    //        await _fileStorage.DeleteAsync(tenant.Logo);
    //}
}
