namespace Appointments.Notifications.Infrastructure.Emails;

internal sealed record BrevoEmailSenderOptions(
    Subject Sender,
    string EmailConfirmationUrl);
