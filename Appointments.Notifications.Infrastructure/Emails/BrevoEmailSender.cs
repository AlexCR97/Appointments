using Appointments.Notifications.Application.Emails;

namespace Appointments.Notifications.Infrastructure.Emails;

internal sealed class BrevoEmailSender : IEmailSender
{
    private readonly BrevoEmailSenderOptions _options;
    private readonly IBrevoApi _brevoApi;

    public BrevoEmailSender(BrevoEmailSenderOptions options, IBrevoApi brevoApi)
    {
        _options = options;
        _brevoApi = brevoApi;
    }

    public async Task SendAsync(
        string subject,
        string toEmail,
        string toName,
        string htmlContent)
    {
        await _brevoApi.SendEmailAsync(new SendEmailRequest(
            subject,
            htmlContent,
            new Subject(
                _options.Sender.Name,
                _options.Sender.Email),
            new Subject[]
            {
                new Subject(toName, toEmail),
            }));
    }

    
}
