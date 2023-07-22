using Appointments.Application.Repositories.Users;
using Appointments.Domain.Models;
using MediatR;

namespace Appointments.Application.Requests.Users;

public record GetUserProfileRequest(
    Guid Id) : IRequest<UserProfile>;

internal class GetUserProfileRequestHandler : IRequestHandler<GetUserProfileRequest, UserProfile>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfile> Handle(GetUserProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        return user.GetProfile();
    }
}
