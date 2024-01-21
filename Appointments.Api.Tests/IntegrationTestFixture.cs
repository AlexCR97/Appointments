using Appointments.Api.Connect.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using Appointments.Common.Domain.Models;
using Bogus;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Appointments.Api.Tests;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollectionFixture : ICollectionFixture<IntegrationTestFixture>
{
    public const string Name = nameof(IntegrationTestCollectionFixture);
}

public sealed class IntegrationTestFixture : IDisposable
{
    private const string _dockerComposeFilePath = "./Appointments.Tests.yml";

    private readonly ICompositeService _compositeService;

    public IntegrationTestFixture()
    {
        // TODO Fix error with Rabbit MQ:
        // RabbitMQ.Client.Exceptions.BrokerUnreachableException: None of the specified endpoints were reachable

        _compositeService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(_dockerComposeFilePath)
            .RemoveOrphans()
            .Build()
            .Start();

        Faker = new Faker();

        HttpClient = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(host =>
            {
                // TODO Figure out how to output to xUnit test output
                //host.ConfigureLogging(logging =>
                //{
                //    logging.ClearProviders();
                //    logging.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
                //});
            })
            .CreateClient();
    }

    public Faker Faker { get; private set; }

    public HttpClient HttpClient { get; private set; }

    public async Task<User> AuthenticateAsync(string? scope = null)
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

    public void Dispose()
    {
        _compositeService.Stop();
        _compositeService.Remove();
    }
}

public sealed record User(
    Guid TenantId,
    Guid UserId,
    Email Email,
    string AccessToken,
    string FirstName,
    string LastName);
