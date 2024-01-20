using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Users;

public sealed record ConfirmEmailRequest(
    string ConfirmationCode)
    : IRequest<EmailConfirmationResult>;

public enum EmailConfirmationResult
{
    /// <summary>
    /// Could not find an email account associated with the confirmation code.
    /// </summary>
    NotFound,

    /// <summary>
    /// The email has already been confirmed previously.
    /// </summary>
    Stale,

    /// <summary>
    /// The confirmation code has expired. A new confirmation code must be issued.
    /// </summary>
    Expired,

    /// <summary>
    /// The email was confirmed successfully.
    /// </summary>
    Confirmed,
}

internal sealed class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.ConfirmationCode)
            .NotEmpty();
    }
}

internal sealed class ConfirmEmailRequestHandler : IRequestHandler<ConfirmEmailRequest, EmailConfirmationResult>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IUserRepository _userRepository;

    public ConfirmEmailRequestHandler(IEventProcessor eventProcessor, IUserRepository userRepository)
    {
        _eventProcessor = eventProcessor;
        _userRepository = userRepository;
    }

    public async Task<EmailConfirmationResult> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        new ConfirmEmailRequestValidator().ValidateAndThrow(request);

        var user = await _userRepository.GetByLocalLoginConfirmationCodeOrDefaultAsync(request.ConfirmationCode);

        if (user is null)
            return EmailConfirmationResult.NotFound;

        var localLogin = user.GetLocalLogin();

        if (localLogin.Confirmed)
            return EmailConfirmationResult.Stale;

        if (localLogin.ConfirmationCodeExpiration < DateTime.UtcNow)
            return EmailConfirmationResult.Expired;

        user.ConfirmLogin(
            localLogin.Email.Value,
            localLogin);

        if (user.HasChanged)
        {
            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
            return EmailConfirmationResult.Confirmed;
        }

        throw new InvalidOperationException($"Operation did not satisfy any result of type {nameof(EmailConfirmationResult)}");
    }
}
