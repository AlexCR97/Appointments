using Appointments.Application.Services.Events;
using MediatR;

namespace Appointments.Application.Requests.Users;

public sealed record UpdateUserProfileRequest(
    Guid Id,
    string UpdatedBy,
    string FirstName,
    string LastName)
    : IRequest;

public class UpdateUserProfileRequestHandler : IRequestHandler<UpdateUserProfileRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IUserRepository _userRepository;

    public UpdateUserProfileRequestHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
    {
        _eventProcessor = eventProcessor;
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.Id);

        user.UpdateProfile(
            request.UpdatedBy,
            request.FirstName,
            request.LastName);

        if (user.HasChanged)
        {
            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}
