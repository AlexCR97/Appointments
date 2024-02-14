using Appointments.Api.Tenant;
using Appointments.Api.Tenant.Models;
using Appointments.Common.Domain.Http;
using Appointments.Common.Domain.Json;
using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using Bogus;
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
            .WithJsonContent(_fixture.Faker.CreateBranchOfficeRequest())
            .Build();

        var response = await _fixture.HttpClient.SendAsync(request);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var branchOfficeCreatedResponse = await response.Content.DeserializeJsonAsync<BranchOfficeCreatedResponse>();
        branchOfficeCreatedResponse.BranchOfficeId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CanFindBranchOffices()
    {
        const int branchOfficesCount = 15;

        var user = await _fixture.AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var createBranchOfficeRequests = _fixture.Faker.CreateBranchOfficeRequests(branchOfficesCount);

        var postTasks = createBranchOfficeRequests
            .Select(request => new HttpRequestMessageBuilder(HttpMethod.Post, $"api/tenant/tenants/{user.TenantId}/branch-offices")
                .WithAccessToken(user.AccessToken)
                .WithJsonContent(request)
                .Build())
            .Select(_fixture.HttpClient.SendAsync);

        var postResponses = await Task.WhenAll(postTasks);
        postResponses.Should().NotBeNullOrEmpty();
        postResponses.Should().HaveCount(createBranchOfficeRequests.Length);
        postResponses.Should().AllSatisfy(response => response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK));

        var branchOfficesResponse = await _fixture.HttpClient.SendAsync(new HttpRequestMessageBuilder(HttpMethod.Get, $"api/tenant/tenants/{user.TenantId}/branch-offices")
            .WithAccessToken(user.AccessToken)
            .WithQuery("pageSize", branchOfficesCount.ToString())
            .Build());
        
        var pagedResult = await branchOfficesResponse.Content.DeserializeJsonAsync<PagedResult<BranchOfficeListResponse>>();
        pagedResult.PageIndex.Should().Be(0);
        pagedResult.PageSize.Should().Be(branchOfficesCount);
        pagedResult.TotalCount.Should().Be(branchOfficesCount + user.BranchOfficesCount);
        pagedResult.Results.Should().HaveCount(branchOfficesCount);
    }

    [Fact]
    public async Task CanGetBranchOfficeProfile()
    {
        var user = await _fixture.AuthenticateAsync(scope: TenantApiPolicy.Tenants.Scope);

        var createBranchOfficeRequest = _fixture.Faker.CreateBranchOfficeRequest();

        var createRequest = new HttpRequestMessageBuilder(HttpMethod.Post, $"api/tenant/tenants/{user.TenantId}/branch-offices")
            .WithAccessToken(user.AccessToken)
            .WithJsonContent(createBranchOfficeRequest)
            .Build();

        var createResponse = await _fixture.HttpClient.SendAsync(createRequest);
        createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var branchOfficeCreatedResponse = await createResponse.Content.DeserializeJsonAsync<BranchOfficeCreatedResponse>();

        var branchOfficeProfileResponse = await _fixture.HttpClient.SendAsync(new HttpRequestMessageBuilder(HttpMethod.Get, $"api/tenant/tenants/{user.TenantId}/branch-offices/{branchOfficeCreatedResponse.BranchOfficeId}")
            .WithAccessToken(user.AccessToken)
            .Build());

        var branchOfficeProfile = await branchOfficeProfileResponse.Content.DeserializeJsonAsync<BranchOfficeProfileResponse>();
        branchOfficeProfile.Address.ShouldMatchAddress(createBranchOfficeRequest.Address);
        branchOfficeProfile.Contacts.ShouldMatchContacts(createBranchOfficeRequest.Contacts!);
        branchOfficeProfile.Name.Should().Be(createBranchOfficeRequest.Name);
        branchOfficeProfile.Schedule.Should().NotBeNull();
        branchOfficeProfile.Schedule!.ShouldMatchWeeklySchedule(createBranchOfficeRequest.Schedule!);
    }

    #endregion
}

file static class Generators
{
    private const int _defaultMax = 5;

    public static CreateBranchOfficeRequest[] CreateBranchOfficeRequests(this Faker faker, int? count = null)
    {
        count ??= faker.Random.Number(_defaultMax);

        return Enumerable
            .Range(0, count.Value)
            .Select(_ => faker.CreateBranchOfficeRequest())
            .ToArray();
    }

    public static CreateBranchOfficeRequest CreateBranchOfficeRequest(this Faker faker)
    {
        return new CreateBranchOfficeRequest(
            faker.Commerce.Department(),
            faker.AddressModel(),
            faker.SocialMediaContactModels(),
            faker.WeeklyScheduleModel());
    }

    private static AddressModel AddressModel(this Faker faker)
    {
        return new AddressModel(
            faker.CoordinatesModel(),
            $"{faker.Address.StreetName()} {faker.Address.BuildingNumber()}, {faker.Address.City()}, {faker.Address.State()}, {faker.Address.ZipCode()}");
    }

    private static CoordinatesModel CoordinatesModel(this Faker faker)
    {
        return new CoordinatesModel(
            faker.Random.Number(min: int.MinValue, max: int.MaxValue),
            faker.Random.Number(min: int.MinValue, max: int.MaxValue));
    }

    private static SocialMediaContactModel[] SocialMediaContactModels(this Faker faker, int? count = null)
    {
        count ??= faker.Random.Number(_defaultMax);

        return Enumerable
            .Range(0, count.Value)
            .Select(_ => faker.SocialMediaContactModel())
            .ToArray();
    }

    private static SocialMediaContactModel SocialMediaContactModel(this Faker faker)
    {
        return new SocialMediaContactModel(
            faker.PickRandom(Enum.GetValues<SocialMediaType>()).ToString(),
            null,
            faker.Internet.UserName());
    }

    private static WeeklyScheduleModel WeeklyScheduleModel(this Faker faker)
    {
        return new WeeklyScheduleModel(
            faker.DailyScheduleModel(),
            faker.DailyScheduleModel(),
            faker.DailyScheduleModel(),
            faker.DailyScheduleModel(),
            faker.DailyScheduleModel(),
            faker.DailyScheduleModel(),
            faker.DailyScheduleModel());
    }

    private static DailyScheduleModel DailyScheduleModel(this Faker faker)
    {
        return new DailyScheduleModel(
            faker.DateRangeModels(),
            faker.Random.Bool());
    }

    private static DateRangeModel[] DateRangeModels(this Faker faker, int? count = null)
    {
        count ??= faker.Random.Number(_defaultMax);

        return Enumerable
            .Range(0, count.Value)
            .Select(_ => faker.DateRangeModel())
            .ToArray();
    }

    private static DateRangeModel DateRangeModel(this Faker faker)
    {
        var dateRange = DateRange.NineToFiveUtc();

        return new DateRangeModel(
            dateRange.StartDate,
            dateRange.EndDate,
            faker.Random.Bool());
    }
}

file static class FluentAssertionsExtensions
{
    public static void ShouldMatchAddress(this AddressModel address, AddressModel other)
    {
        address.Coordinates.ShouldMatchCoordinates(other.Coordinates);
        address.Description.Should().Be(other.Description);
    }

    public static void ShouldMatchCoordinates(this CoordinatesModel coordinates, CoordinatesModel other)
    {
        coordinates.Latitude.Should().Be(other.Latitude);
        coordinates.Longitude.Should().Be(other.Longitude);
    }

    public static void ShouldMatchContacts(this SocialMediaContactModel[] contacts, SocialMediaContactModel[] other)
    {
        contacts.Length.Should().Be(other.Length);

        for (var index = 0; index < contacts.Length;  index++)
        {
            contacts[index].Type.Should().Be(other[index].Type);
            contacts[index].OtherType.Should().Be(other[index].OtherType);
            contacts[index].Contact.Should().Be(other[index].Contact);
        }
    }

    public static void ShouldMatchWeeklySchedule(this WeeklyScheduleModel schedule, WeeklyScheduleModel other)
    {
        schedule.Monday.Should().NotBeNull();
        schedule.Monday.ShouldMatchDailySchedule(other.Monday);

        schedule.Tuesday.Should().NotBeNull();
        schedule.Tuesday.ShouldMatchDailySchedule(other.Tuesday);

        schedule.Wednesday.Should().NotBeNull();
        schedule.Wednesday.ShouldMatchDailySchedule(other.Wednesday);

        schedule.Thursday.Should().NotBeNull();
        schedule.Thursday.ShouldMatchDailySchedule(other.Thursday);

        schedule.Friday.Should().NotBeNull();
        schedule.Friday.ShouldMatchDailySchedule(other.Friday);

        schedule.Saturday.Should().NotBeNull();
        schedule.Saturday.ShouldMatchDailySchedule(other.Saturday);

        schedule.Sunday.Should().NotBeNull();
        schedule.Sunday.ShouldMatchDailySchedule(other.Sunday);
    }

    public static void ShouldMatchDailySchedule(this DailyScheduleModel schedule, DailyScheduleModel other)
    {
        schedule.Disabled.Should().Be(other.Disabled);

        if (schedule.Hours is not null)
        {
            other.Hours.Should().NotBeNull();

            schedule.Hours.Length.Should().Be(other.Hours!.Length);

            for (var index = 0; index < schedule.Hours.Length; index++)
            {
                schedule.Hours[index].StartDate.Should().Be(other.Hours[index].StartDate);
                schedule.Hours[index].EndDate.Should().Be(other.Hours[index].EndDate);
                schedule.Hours[index].Disabled.Should().Be(other.Hours[index].Disabled);
            }
        }
    }
}
