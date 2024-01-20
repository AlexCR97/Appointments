using System.Text;

namespace Appointments.Notifications.Infrastructure.Emails.HtmlTemplates;

internal sealed class HtmlEmbeddedResource
{
    public string Subject { get; }
    public string Name { get; }
    private string FullPath => $"Emails.HtmlTemplates.{Name}.html";

    public HtmlEmbeddedResource(string subject, string name)
    {
        Subject = subject;
        Name = name;
    }

    public string ReadContent(Dictionary<string, string>? data = null)
    {
        var content = EmbeddedResourceReader.Read(GetType().Assembly, FullPath);

        return new StringBuilder()
            .Append(content)
            .Replace(data)
            .ToString();
    }
}

internal static class StringBuilderExtensions
{
    public static StringBuilder Replace(this StringBuilder builder, Dictionary<string, string>? values)
    {
        if (values is not null && values.Count > 0)
        {
            foreach (var pair in values)
            {
                builder.Replace(pair.Key, pair.Value);
            }
        }

        return builder;
    }
}
