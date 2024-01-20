using System.Reflection;

namespace Appointments.Notifications.Infrastructure.Emails.HtmlTemplates;

internal static class EmbeddedResourceReader
{
    public static string Read(Assembly assembly, string resourceName)
    {
        var assemblyName = assembly
            .GetName()
            .Name
            ?? throw new InvalidOperationException("The executing assembly does not have a name");

        var completeResourceName = $"{assemblyName}.{resourceName}";

        using var stream = assembly
            .GetManifestResourceStream(completeResourceName)
            ?? throw new InvalidOperationException(@$"No such embedded resource ""{completeResourceName}""");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
