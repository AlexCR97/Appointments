namespace Appointments.Notifications.Application.Emails;

public interface IEmailSender
{
    Task SendAsync(
        string subject,
        string toEmail,
        string toName,
        string htmlContent);
}
