using Appointments.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Users;

public sealed record GetUserRequest : GetRequest<User>
{
    public GetUserRequest(Guid Id) : base(Id)
    {
    }
}

internal sealed class GetUserRequestHandler : IRequestHandler<GetUserRequest, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        new GetRequestValidator<User>().ValidateAndThrow(request);

        return await _userRepository.GetAsync(request.Id);
    }
}
