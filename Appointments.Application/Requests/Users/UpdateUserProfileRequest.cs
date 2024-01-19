using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Users;

public sealed record UpdateUserProfileRequest(
    Guid Id,
    string UpdatedBy,
    string FirstName,
    string LastName)
    : IRequest;

internal sealed class UpdateUserProfileRequestValidator : AbstractValidator<UpdateUserProfileRequest>
{
    public UpdateUserProfileRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}

internal sealed class UpdateUserProfileRequestHandler : IRequestHandler<UpdateUserProfileRequest>
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
        new UpdateUserProfileRequestValidator().ValidateAndThrow(request);

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
