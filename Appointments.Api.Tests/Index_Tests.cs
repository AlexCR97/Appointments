using FluentAssertions;
using System.Text.Json.Nodes;
using Xunit;

namespace Appointments.Api.Tests;

[Collection(IntegrationTestCollectionFixture.Name)]
public sealed class Index_Tests
{
    private readonly IntegrationTestFixture _fixture;

    public Index_Tests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CanGetSwaggerScheme()
    {
        var response = await _fixture.HttpClient.GetAsync("swagger/v1/swagger.json");

        response.IsSuccessStatusCode.Should().BeTrue();

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrWhiteSpace();

        var responseContentJson = JsonNode.Parse(responseContent);
        responseContentJson.Should().NotBeNull();
    }
}
