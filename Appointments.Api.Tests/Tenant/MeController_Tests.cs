using Appointments.Api.Connect.Models;
using Appointments.Api.Tenant;
using Appointments.Api.Tenant.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using Appointments.Common.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Appointments.Api.Tests.Tenant;

public sealed class MeController_Tests : IntegrationTest
{
    public MeController_Tests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }

    [Fact]
    public async Task CanGetMyProfile()
    {
        var user = await AuthenticateAsync(scope: TenantApiPolicy.Me.Scope);

        var request = new HttpRequestMessage(HttpMethod.Get, "api/tenant/me");
        request.Headers.Add("Authorization", $"Bearer {user.AccessToken}");

        var response = await HttpClient.SendAsync(request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var userProfileResponse = await response.Content.DeserializeJsonAsync<UserProfileResponse>();
        userProfileResponse.Id.Should().Be(user.UserId);
        userProfileResponse.Tenants.Should().ContainSingle(x => x.TenantId == user.TenantId);
        userProfileResponse.Logins.Should().ContainSingle(x => x.Email == user.Email.Value);
    }

    [Fact]
    public async Task CanUpdateMyProfile()
    {
        var user = await AuthenticateAsync(scope: TenantApiPolicy.Me.Scope);

        var updateUserProfileRequest = new UpdateUserProfileRequest(
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var updateMyProfileRequest = new HttpRequestMessage(HttpMethod.Put, "api/tenant/me");
        updateMyProfileRequest.Headers.Add("Authorization", $"Bearer {user.AccessToken}");
        updateMyProfileRequest.Content = updateUserProfileRequest.ToJsonContent();

        var updateMyProfileResponse = await HttpClient.SendAsync(updateMyProfileRequest);
        updateMyProfileResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var myProfileRequest = new HttpRequestMessage(HttpMethod.Get, "api/tenant/me");
        myProfileRequest.Headers.Add("Authorization", $"Bearer {user.AccessToken}");

        var myProfileResponse = await HttpClient.SendAsync(myProfileRequest);
        myProfileResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var userProfileResponse = await myProfileResponse.Content.DeserializeJsonAsync<UserProfileResponse>();
        userProfileResponse.FirstName.Should().Be(updateUserProfileRequest.FirstName);
        userProfileResponse.LastName.Should().Be(updateUserProfileRequest.LastName);
    }

    private async Task<User> AuthenticateAsync(
        string? scope = null)
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
                null)
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

internal sealed record User(
    Guid TenantId,
    Guid UserId,
    Email Email,
    string AccessToken,
    string FirstName,
    string LastName);
