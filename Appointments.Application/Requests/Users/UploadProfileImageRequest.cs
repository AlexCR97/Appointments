using Appointments.Application.Repositories.Users;
using Appointments.Application.Services.Events;
using Appointments.Application.Services.Files;
using Appointments.Domain.Entities;
using MediatR;

namespace Appointments.Application.Requests.Users;

public sealed record UploadProfileImageRequest(
    Guid Id,
    string FileName,
    byte[] File): IRequest<string>;

internal sealed class UploadProfileImageRequestHandler : IRequestHandler<UploadProfileImageRequest, string>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IFileStorage _fileStorage;
    private readonly IUserRepository _userRepository;

    public UploadProfileImageRequestHandler(IEventProcessor eventProcessor, IFileStorage fileStorage, IUserRepository userRepository)
    {
        _eventProcessor = eventProcessor;
        _fileStorage = fileStorage;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(UploadProfileImageRequest request, CancellationToken cancellationToken)
    {
        // TODO Validate request

        var user = await _userRepository.GetByIdAsync(request.Id);

        var imageRelativePathWithoutFileName = Path.Join("Users", user.Id.ToString());
        await _fileStorage.EnsureDirectoryAsync(imageRelativePathWithoutFileName);

        var imageName = GetImageName(request);
        var imageRelativePathWithFileName = Path.Join(imageRelativePathWithoutFileName, imageName);
        
        await TryDeletePreviousProfileImageAsync(user);

        await _fileStorage.WriteAsync(imageRelativePathWithFileName, request.File);

        user.UpdateProfileImage(
            user.Email,
            imageRelativePathWithFileName);

        if (user.HasChanged)
        {
            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }

        return imageRelativePathWithFileName;
    }

    private static string GetImageName(UploadProfileImageRequest request)
    {
        var fileExtension = request.FileName
            .Split('.')
            .Last();

        return $"ProfileImage.{fileExtension}";
    }

    private async Task TryDeletePreviousProfileImageAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.ProfileImage))
            return;

        if (await _fileStorage.ExistsAsync(user.ProfileImage))
            await _fileStorage.DeleteAsync(user.ProfileImage);
    }
}
