using Appointments.Application.Services.Events;
using Appointments.Application.Services.Files;
using Appointments.Application.Validations.Services;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Services;

public sealed record UploadImagesRequest(
    string? UpdatedBy,
    Guid Id,
    IReadOnlyList<UploadImagesRequest.IndexedImage> Images
    ): IRequest<IReadOnlyList<IndexedResource>>
{
    public sealed record IndexedImage(
        int Index,
        string FileName,
        byte[] File);
}

internal sealed class UploadImagesRequestHandler : IRequestHandler<UploadImagesRequest, IReadOnlyList<IndexedResource>>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IFileStorage _fileStorage;
    private readonly IServiceRepository _serviceRepository;

    public UploadImagesRequestHandler(IEventProcessor eventProcessor, IFileStorage fileStorage, IServiceRepository serviceRepository)
    {
        _eventProcessor = eventProcessor;
        _fileStorage = fileStorage;
        _serviceRepository = serviceRepository;
    }

    public async Task<IReadOnlyList<IndexedResource>> Handle(UploadImagesRequest request, CancellationToken cancellationToken)
    {
        new UploadImagesRequestValidator().ValidateAndThrow(request);

        var service = await _serviceRepository.GetAsync(request.Id);

        // TODO Delete images that are no longer references
        // TODO Create only new images, not already existing ones

        var indexedImages = await UploadImagesAsync(
            request.Images.OrderBy(x => x.Index).ToList(),
            service);

        service.UpdateImages(
            request.UpdatedBy,
            indexedImages);

        if (service.HasChanged)
        {
            await _serviceRepository.UpdateAsync(service);
            await _eventProcessor.ProcessAsync(service.Events);
        }

        return service.Images;
    }

    private async Task<List<IndexedResource>> UploadImagesAsync(IReadOnlyList<UploadImagesRequest.IndexedImage> images, Service service)
    {
        var indexedImages = new List<IndexedResource>();

        foreach (var image in images)
        {
            var indexedImage = await UploadImageAsync(service, image);
            indexedImages.Add(indexedImage);
        }

        return indexedImages;
    }

    private async Task<IndexedResource> UploadImageAsync(Service service, UploadImagesRequest.IndexedImage image)
    {
        var imageRelativePathWithoutFileName = Path.Join("Services", service.Id.ToString());
        await _fileStorage.EnsureDirectoryAsync(imageRelativePathWithoutFileName);

        var imageName = GetImageName(image);
        var imageRelativePathWithFileName = Path.Join(imageRelativePathWithoutFileName, imageName);

        await _fileStorage.WriteAsync(imageRelativePathWithFileName, image.File);

        return new IndexedResource(
            image.Index,
            imageRelativePathWithFileName);
    }

    private static string GetImageName(UploadImagesRequest.IndexedImage image)
    {
        var fileExtension = image.FileName
            .Split('.')
            .Last();

        return $"Image-{image.Index}.{fileExtension}";
    }
}
