using Appointments.Application.Services.Events;
using Appointments.Application.Services.Files;
using Appointments.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Users;

public sealed record UploadUserProfileImageRequest(
    Guid Id,
    string UpdatedBy,
    string FileName,
    byte[] File)
    : IRequest<string>;

internal sealed class UploadUserProfileImageRequestValidator : AbstractValidator<UploadUserProfileImageRequest>
{
    public UploadUserProfileImageRequestValidator()
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

internal sealed class UploadUserProfileImageRequestHandler : IRequestHandler<UploadUserProfileImageRequest, string>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IFileStorage _fileStorage;
    private readonly IUserRepository _userRepository;

    public UploadUserProfileImageRequestHandler(IEventProcessor eventProcessor, IFileStorage fileStorage, IUserRepository userRepository)
    {
        _eventProcessor = eventProcessor;
        _fileStorage = fileStorage;
        _userRepository = userRepository;
    }

    public async Task<string> Handle(UploadUserProfileImageRequest request, CancellationToken cancellationToken)
    {
        // TODO Validate request
        new UploadUserProfileImageRequestValidator().ValidateAndThrow(request);

        var user = await _userRepository.GetAsync(request.Id);

        var imageRelativePathWithoutFileName = Path.Join("Users", user.Id.ToString());
        await _fileStorage.EnsureDirectoryAsync(imageRelativePathWithoutFileName);

        var imageName = GetImageName(request);
        var imageRelativePathWithFileName = Path.Join(imageRelativePathWithoutFileName, imageName);
        
        await TryDeletePreviousProfileImageAsync(user);

        await _fileStorage.WriteAsync(imageRelativePathWithFileName, request.File);

        user.UpdateProfileImage(
            request.UpdatedBy,
            imageRelativePathWithFileName);

        if (user.HasChanged)
        {
            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }

        return imageRelativePathWithFileName;
    }

    private static string GetImageName(UploadUserProfileImageRequest request)
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
