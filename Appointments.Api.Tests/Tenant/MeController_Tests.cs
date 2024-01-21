using Appointments.Api.Tenant;
using Appointments.Api.Tenant.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using FluentAssertions;
using Xunit;

namespace Appointments.Api.Tests.Tenant;

[Collection(IntegrationTestCollectionFixture.Name)]
public sealed class MeController_Tests
{
    private readonly IntegrationTestFixture _fixture;

    public MeController_Tests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CanGetMyProfile()
    {
        var user = await _fixture.AuthenticateAsync(scope: TenantApiPolicy.Me.Scope);

        var request = new HttpRequestMessage(HttpMethod.Get, "api/tenant/me");
        request.Headers.Add("Authorization", $"Bearer {user.AccessToken}");

        var response = await _fixture.HttpClient.SendAsync(request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var userProfileResponse = await response.Content.DeserializeJsonAsync<UserProfileResponse>();
        userProfileResponse.Id.Should().Be(user.UserId);
        userProfileResponse.Tenants.Should().ContainSingle(x => x.TenantId == user.TenantId);
        userProfileResponse.Logins.Should().ContainSingle(x => x.Email == user.Email.Value);
    }

    [Fact]
    public async Task CanUpdateMyProfile()
    {
        var user = await _fixture.AuthenticateAsync(scope: TenantApiPolicy.Me.Scope);

        var updateUserProfileRequest = new UpdateUserProfileRequest(
            _fixture.Faker.Name.FirstName(),
            _fixture.Faker.Name.LastName());

        var updateMyProfileRequest = new HttpRequestMessage(HttpMethod.Put, "api/tenant/me");
        updateMyProfileRequest.Headers.Add("Authorization", $"Bearer {user.AccessToken}");
        updateMyProfileRequest.Content = updateUserProfileRequest.ToJsonContent();

        var updateMyProfileResponse = await _fixture.HttpClient.SendAsync(updateMyProfileRequest);
        updateMyProfileResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var myProfileRequest = new HttpRequestMessage(HttpMethod.Get, "api/tenant/me");
        myProfileRequest.Headers.Add("Authorization", $"Bearer {user.AccessToken}");

        var myProfileResponse = await _fixture.HttpClient.SendAsync(myProfileRequest);
        myProfileResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var userProfileResponse = await myProfileResponse.Content.DeserializeJsonAsync<UserProfileResponse>();
        userProfileResponse.FirstName.Should().Be(updateUserProfileRequest.FirstName);
        userProfileResponse.LastName.Should().Be(updateUserProfileRequest.LastName);
    }
}
