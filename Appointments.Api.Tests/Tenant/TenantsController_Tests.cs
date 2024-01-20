using Appointments.Api.Tenant;
using Appointments.Api.Tenant.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using Appointments.Core.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Appointments.Api.Tests.Tenant;

[Collection(IntegrationTestCollectionFixture.Name)]
public sealed class TenantsController_Tests
{
    private readonly IntegrationTestFixture _fixture;

    public TenantsController_Tests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ShouldRespondWithForbiddenIfTokenNotTenantScoped()
    {
        var user = await _fixture.AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/tenant/tenants/{Guid.NewGuid()}");
        request.Headers.Add("Authorization", $"Bearer {user.AccessToken}");

        var response = await _fixture.HttpClient.SendAsync(request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    #region Tenants

    [Fact]
    public async Task CanGetTenantProfile()
    {
        var user = await _fixture.AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/tenant/tenants/{user.TenantId}");
        request.Headers.Add("Authorization", $"Bearer {user.AccessToken}");

        var response = await _fixture.HttpClient.SendAsync(request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var tenantProfileResponse = await response.Content.DeserializeJsonAsync<TenantProfileResponse>();
        tenantProfileResponse.Id.Should().Be(user.TenantId);
        tenantProfileResponse.Name.Should().NotBeNullOrWhiteSpace();
        tenantProfileResponse.UrlId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task CanUpdateTenantProfile()
    {
        var user = await _fixture.AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var updateTenantRequest = new UpdateTenantProfileRequest(
            _fixture.Faker.Company.CompanyName(),
            _fixture.Faker.Company.CatchPhrase(),
            TenantUrlId.Random().Value,
            null);

        var updateTenantRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/tenant/tenants/{user.TenantId}");
        updateTenantRequestMessage.Headers.Add("Authorization", $"Bearer {user.AccessToken}");
        updateTenantRequestMessage.Content = updateTenantRequest.ToJsonContent();
        var updateTenantResponse = await _fixture.HttpClient.SendAsync(updateTenantRequestMessage);
        updateTenantResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var getTenantRequest = new HttpRequestMessage(HttpMethod.Get, $"api/tenant/tenants/{user.TenantId}");
        getTenantRequest.Headers.Add("Authorization", $"Bearer {user.AccessToken}");
        var getTenantResponse = await _fixture.HttpClient.SendAsync(getTenantRequest);
        var tenantProfileResponse = await getTenantResponse.Content.DeserializeJsonAsync<TenantProfileResponse>();

        tenantProfileResponse.Name.Should().Be(updateTenantRequest.Name);
        tenantProfileResponse.Slogan.Should().Be(updateTenantRequest.Slogan);
        tenantProfileResponse.UrlId.Should().Be(updateTenantRequest.UrlId);
    }

    #endregion

    #region Branch Offices

    [Fact]
    public async Task CanCreateBranchOffice()
    {
        var user = await _fixture.AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var request = new HttpRequestMessageBuilder(HttpMethod.Post, $"api/tenant/tenants/{user.TenantId}/branch-offices")
            .WithAccessToken(user.AccessToken)
            .WithJsonContent(new CreateBranchOfficeRequest(
                _fixture.Faker.Company.CompanyName(),
                new AddressModel(
                    new CoordinatesModel(0, 0),
                    string.Empty),
                null,
                null))
            .Build();

        var response = await _fixture.HttpClient.SendAsync(request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var branchOfficeCreatedResponse = await response.Content.DeserializeJsonAsync<BranchOfficeCreatedResponse>();
        branchOfficeCreatedResponse.BranchOfficeId.Should().NotBeEmpty();
    }

    #endregion
}
