using Bogus;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Appointments.Api.Tests;

public abstract class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected IntegrationTest(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        Faker = new Faker();

        HttpClient = factory
            .WithWebHostBuilder(host =>
            {
                host.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
                });
            })
            .CreateClient();
    }

    public Faker Faker { get; private set; }

    public HttpClient HttpClient { get; private set; }
}
