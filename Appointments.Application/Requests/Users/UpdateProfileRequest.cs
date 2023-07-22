using Appointments.Application.Repositories.Users;
using Appointments.Application.Services.Events;
using MediatR;

namespace Appointments.Application.Requests.Users;

public record UpdateProfileRequest(
    Guid Id,
    string FirstName,
    string LastName) : IRequest;

public class UpdateProfileRequestHandler : IRequestHandler<UpdateProfileRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IUserRepository _userRepository;

    public UpdateProfileRequestHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
    {
        _eventProcessor = eventProcessor;
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        user.UpdateProfile(
            user.Email,
            request.FirstName,
            request.LastName);

        if (user.HasChanged)
        {
            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }
}
