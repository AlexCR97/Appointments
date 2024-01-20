using Appointments.Api.Connect.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using FluentAssertions;
using Xunit;

namespace Appointments.Api.Tests.Connect;

[Collection(IntegrationTestCollectionFixture.Name)]
public sealed class ConnectController_Tests
{
    private readonly IntegrationTestFixture _fixture;

    public ConnectController_Tests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CanSignUpWithEmail()
    {
        var password = _fixture.Faker.Internet.Password();

        var response = await _fixture.HttpClient.PostAsync(
            "api/connect/sign-up/email",
            new SignUpWithEmailRequest(
                _fixture.Faker.Name.FirstName(),
                _fixture.Faker.Name.LastName(),
                _fixture.Faker.Internet.Email(),
                password,
                password,
                _fixture.Faker.Company.CompanyName())
            .ToJsonContent());

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var userSignUpResponse = await response.Content.DeserializeJsonAsync<UserSignedUpResponse>();
        userSignUpResponse.UserId.Should().NotBeEmpty();
        userSignUpResponse.TenantId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CanLoginWithEmail()
    {
        var password = _fixture.Faker.Internet.Password();
        var signUpWithEmailRequest = new SignUpWithEmailRequest(
            _fixture.Faker.Name.FirstName(),
            _fixture.Faker.Name.LastName(),
            _fixture.Faker.Internet.Email(),
            password,
            password,
            _fixture.Faker.Company.CompanyName());

        var signUpResponse = await _fixture.HttpClient.PostAsync(
            "api/connect/sign-up/email",
            signUpWithEmailRequest.ToJsonContent());

        signUpResponse.IsSuccessStatusCode.Should().BeTrue();

        var loginResponse = await _fixture.HttpClient.PostAsync(
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
