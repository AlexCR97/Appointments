using System.Net.Http.Json;

namespace Appointments.Notifications.Infrastructure.Emails;

internal sealed class BrevoApi
{
    private readonly HttpClient _httpClient;

    public BrevoApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendEmailAsync(SendEmailRequest request)
    {
        var response = await _httpClient.PostAsync("v3/smtp/email", JsonContent.Create(request));

        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync(); // Get response content to debug
            response.EnsureSuccessStatusCode(); // Throw exception
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
