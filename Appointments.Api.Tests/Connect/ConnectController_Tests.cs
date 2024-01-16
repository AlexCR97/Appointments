using Appointments.Api.Connect.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Appointments.Api.Tests.Connect;

public sealed class ConnectController_Tests : IntegrationTest
{
    public ConnectController_Tests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }

    [Fact]
    public async Task CanSignUpWithEmail()
    {
        var password = Faker.Internet.Password();

        var response = await HttpClient.PostAsync(
            "api/connect/sign-up/email",
            new SignUpWithEmailRequest(
                Faker.Name.FirstName(),
                Faker.Name.LastName(),
                Faker.Internet.Email(),
                password,
                password,
                Faker.Company.CompanyName())
            .ToJsonContent());

        response.IsSuccessStatusCode.Should().BeTrue();

        var userSignUpResponse = await response.Content.DeserializeJsonAsync<UserSignedUpResponse>();
        userSignUpResponse.UserId.Should().NotBeEmpty();
        userSignUpResponse.TenantId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CanLoginWithEmail()
    {
        var password = Faker.Internet.Password();
        var signUpWithEmailRequest = new SignUpWithEmailRequest(
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Faker.Internet.Email(),
            password,
            password,
            Faker.Company.CompanyName());

        var signUpResponse = await HttpClient.PostAsync(
            "api/connect/sign-up/email",
            signUpWithEmailRequest.ToJsonContent());

        signUpResponse.IsSuccessStatusCode.Should().BeTrue();

        var loginResponse = await HttpClient.PostAsync(
            "api/connect/login/email",
            new LoginWithEmailRequest(
                signUpWithEmailRequest.Email,
                password,
                null,
                null)
            .ToJsonContent());

        loginResponse.IsSuccessStatusCode.Should().BeTrue();

        var oAuthTokenResponse = await loginResponse.Content.DeserializeJsonAsync<OAuthTokenResponse>();
        oAuthTokenResponse.access_token.Should().NotBeNullOrWhiteSpace();
    }
}
