using Appointments.Core.Contracts.Users;
using Appointments.Notifications.Application.Emails;
using Appointments.Notifications.Infrastructure.Emails;
using Appointments.Notifications.Infrastructure.Emails.HtmlTemplates;
using MassTransit;

namespace Appointments.Notifications.Infrastructure.UseCases.Users;

internal sealed class UserSignedUpWithEmailConsumer : IConsumer<UserSignedUpWithEmailEvent>
{
    private readonly BrevoEmailSenderOptions _brevoEmailSenderOptions;
    private readonly IEmailSender _emailSender;

    public UserSignedUpWithEmailConsumer(BrevoEmailSenderOptions brevoEmailSenderOptions, IEmailSender emailSender)
    {
        _brevoEmailSenderOptions = brevoEmailSenderOptions;
        _emailSender = emailSender;
    }

    public async Task Consume(ConsumeContext<UserSignedUpWithEmailEvent> context)
    {
        await _emailSender.SendAsync(
            HtmlTemplates.EmailConfirmation.Subject,
            context.Message.Email,
            context.Message.FullName,
            HtmlTemplates.EmailConfirmation.ReadContent(new Dictionary<string, string>
            {
                ["FULL_NAME"] = context.Message.FullName,
                ["EMAIL_CONFIRMATION_URL"] = _brevoEmailSenderOptions.EmailConfirmationUrl
                    .Replace("CODE", context.Message.ConfirmationCode),
            }));
    }
}
