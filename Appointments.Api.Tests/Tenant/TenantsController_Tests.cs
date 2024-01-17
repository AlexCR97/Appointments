using Appointments.Api.Tenant;
using Appointments.Api.Tenant.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using Appointments.Core.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Appointments.Api.Tests.Tenant;

public sealed class TenantsController_Tests : IntegrationTest
{
    public TenantsController_Tests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }

    [Fact]
    public async Task ShouldRespondWithForbiddenIfTokenNotTenantScoped()
    {
        var user = await AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/tenant/tenants/{Guid.NewGuid()}");
        request.Headers.Add("Authorization", $"Bearer {user.AccessToken}");

        var response = await HttpClient.SendAsync(request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CanGetTenantProfile()
    {
        var user = await AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/tenant/tenants/{user.TenantId}");
        request.Headers.Add("Authorization", $"Bearer {user.AccessToken}");

        var response = await HttpClient.SendAsync(request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var tenantProfileResponse = await response.Content.DeserializeJsonAsync<TenantProfileResponse>();
        tenantProfileResponse.Id.Should().Be(user.TenantId);
        tenantProfileResponse.Name.Should().NotBeNullOrWhiteSpace();
        tenantProfileResponse.UrlId.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task CanUpdateTenantProfile()
    {
        var user = await AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var updateTenantRequest = new UpdateTenantProfileRequest(
            Faker.Company.CompanyName(),
            Faker.Company.CatchPhrase(),
            TenantUrlId.Random().Value,
            null);

        var updateTenantRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/tenant/tenants/{user.TenantId}");
        updateTenantRequestMessage.Headers.Add("Authorization", $"Bearer {user.AccessToken}");
        updateTenantRequestMessage.Content = updateTenantRequest.ToJsonContent();
        var updateTenantResponse = await HttpClient.SendAsync(updateTenantRequestMessage);
        updateTenantResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var getTenantRequest = new HttpRequestMessage(HttpMethod.Get, $"api/tenant/tenants/{user.TenantId}");
        getTenantRequest.Headers.Add("Authorization", $"Bearer {user.AccessToken}");
        var getTenantResponse = await HttpClient.SendAsync(getTenantRequest);
        var tenantProfileResponse = await getTenantResponse.Content.DeserializeJsonAsync<TenantProfileResponse>();

        tenantProfileResponse.Name.Should().Be(updateTenantRequest.Name);
        tenantProfileResponse.Slogan.Should().Be(updateTenantRequest.Slogan);
        tenantProfileResponse.UrlId.Should().Be(updateTenantRequest.UrlId);
    }
}
