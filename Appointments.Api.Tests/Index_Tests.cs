using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json.Nodes;
using Xunit;

namespace Appointments.Api.Tests;

public class Index_Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public Index_Tests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CanGetSwaggerScheme()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("swagger/v1/swagger.json");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrWhiteSpace();

        var responseContentJson = JsonNode.Parse(responseContent);
        responseContentJson.Should().NotBeNull();
    }
}
