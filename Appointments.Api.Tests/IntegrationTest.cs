using Appointments.Api.Connect.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using Appointments.Common.Domain.Models;
using Bogus;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Appointments.Api.Tests;

public abstract class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected IntegrationTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        Faker = new Faker();

        HttpClient = factory
            .WithWebHostBuilder(host =>
            {
                host.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
                });
            })
            .CreateClient();
    }

    public Faker Faker { get; private set; }

    public HttpClient HttpClient { get; private set; }

    protected async Task<User> AuthenticateAsync(string? scope = null)
    {
        var firstName = Faker.Name.FirstName();
        var lastName = Faker.Name.LastName();
        var email = new Email(Faker.Internet.Email(firstName: firstName, lastName: lastName));
        var password = Faker.Internet.Password();

        var signUpWithEmailRequest = new SignUpWithEmailRequest(
            firstName,
            lastName,
            email.Value,
            password,
            password,
            Faker.Company.CompanyName());

        var signUpResponse = await HttpClient.PostAsync(
            "api/connect/sign-up/email",
            signUpWithEmailRequest.ToJsonContent());

        var userSignedUpResponse = await signUpResponse.Content.DeserializeJsonAsync<UserSignedUpResponse>();

        var loginResponse = await HttpClient.PostAsync(
            "api/connect/login/email",
            new LoginWithEmailRequest(
                email.Value,
                password,
                scope,
                userSignedUpResponse.TenantId)
            .ToJsonContent());

        var oAuthTokenResponse = await loginResponse.Content.DeserializeJsonAsync<OAuthTokenResponse>();

        return new User(
            userSignedUpResponse.TenantId,
            userSignedUpResponse.UserId,
            email,
            oAuthTokenResponse.access_token,
            firstName,
            lastName);
    }
}

public sealed record User(
    Guid TenantId,
    Guid UserId,
    Email Email,
    string AccessToken,
    string FirstName,
    string LastName);
