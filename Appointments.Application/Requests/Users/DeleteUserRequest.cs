using Appointments.Common.Application;
using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Users;

public sealed record DeleteUserRequest : DeleteRequest
{
    public DeleteUserRequest(string DeletedBy, Guid Id) : base(DeletedBy, Id)
    {
    }
}

internal sealed class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IUserRepository _userRepository;

    public DeleteUserRequestHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
    {
        _eventProcessor = eventProcessor;
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        new DeleteRequestValidator().ValidateAndThrow(request);

        var user = await _userRepository.GetAsync(request.Id);

        await _userRepository.DeleteAsync(user.Id);
        await _eventProcessor.ProcessAsync(user.Events);
    }
}
