using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Users;

public sealed record GetUserByEmailRequest(
    Email Email)
    : IRequest<User>;

internal sealed class GetUserByEmailRequestValidator : AbstractValidator<GetUserByEmailRequest>
{
    public GetUserByEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .SetValidator(new EmailValidator());
    }
}

internal sealed class GetUserByEmailRequestHandler : IRequestHandler<GetUserByEmailRequest, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailRequestHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserByEmailRequest request, CancellationToken cancellationToken)
    {
        new GetUserByEmailRequestValidator().ValidateAndThrow(request);

        return await _userRepository.GetByEmailAsync(request.Email);
    }
}
