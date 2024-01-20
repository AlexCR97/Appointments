namespace Appointments.Notifications.Infrastructure.Emails.HtmlTemplates;

internal static class HtmlTemplates
{
    public static readonly HtmlEmbeddedResource EmailConfirmation = new(
        "Welcome to Appointments!",
        "EmailConfirmation");
}
