using Appointments.Core.Contracts.Users;
using Appointments.Notifications.Application.Emails;
using Appointments.Notifications.Infrastructure.Emails;
using Appointments.Notifications.Infrastructure.Emails.HtmlTemplates;
using MassTransit;

namespace Appointments.Notifications.Infrastructure.UseCases.Users;

internal sealed class UserSignedUpWithEmailConsumer : IConsumer<UserSignedUpWithEmailEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly BrevoEmailSenderOptions _brevoEmailSenderOptions;

    public UserSignedUpWithEmailConsumer(IEmailSender emailSender, BrevoEmailSenderOptions brevoEmailSenderOptions)
    {
        _emailSender = emailSender;
        _brevoEmailSenderOptions = brevoEmailSenderOptions;
    }

    public async Task Consume(ConsumeContext<UserSignedUpWithEmailEvent> context)
    {
        try
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
        catch (Exception ex)
        {
            // TODO Handle exception
        }
    }
}
