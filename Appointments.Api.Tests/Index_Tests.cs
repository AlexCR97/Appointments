using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Appointments.Api.Tests;

public sealed class Index_Tests : IntegrationTest
{
    public Index_Tests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory, testOutputHelper)
    {
    }

    [Fact]
    public async Task CanGetSwaggerScheme()
    {
        var response = await HttpClient.GetAsync("swagger/v1/swagger.json");

        response.IsSuccessStatusCode.Should().BeTrue();

        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrWhiteSpace();

        var responseContentJson = JsonNode.Parse(responseContent);
        responseContentJson.Should().NotBeNull();
    }
}
