using System.Net.Http.Json;

namespace Appointments.Notifications.Infrastructure.Emails;

internal interface IBrevoApi
{
    Task SendEmailAsync(SendEmailRequest request);
}

internal sealed class BrevoApi : IBrevoApi
{
    private readonly HttpClient _httpClient;

    public BrevoApi(BrevoApiOptions options)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.brevo.com"),
        };

        _httpClient.DefaultRequestHeaders.Add("api-key", options.ApiKey);
    }

    public async Task SendEmailAsync(SendEmailRequest request)
    {
        var response = await _httpClient.PostAsync("v3/smtp/email", JsonContent.Create(request));

        if (!response.IsSuccessStatusCode)
        {
            // TODO Throw exception
        }
    }

}

public sealed record SendEmailRequest(
    string Subject,
    string HtmlContent,
    Subject Sender,
    Subject[] To);

public sealed record Subject(
    string Name,
    string Email);
