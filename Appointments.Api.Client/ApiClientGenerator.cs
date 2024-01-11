using Appointments.Api.Client.PowerShell;
using Microsoft.OpenApi.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace Appointments.Api.Client;

internal interface IApiClientGenerator
{
    Task GenerateAsync(CancellationToken cancellationToken = new());
}

internal sealed class ApiClientGenerator : IApiClientGenerator
{
    private readonly IPowerShellScript _powerShellScript;
    private readonly ISwaggerProvider _swaggerProvider;

    public ApiClientGenerator(IPowerShellScript powerShellScript, ISwaggerProvider swaggerProvider)
    {
        _powerShellScript = powerShellScript;
        _swaggerProvider = swaggerProvider;
    }

    public async Task GenerateAsync(CancellationToken cancellationToken = new())
    {
        var openApiDocument = _swaggerProvider.GetSwagger("v1", null, "/");
        var swaggerJson = openApiDocument.SerializeAsJson(Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
        File.WriteAllText("swagger.json", swaggerJson);

        var result = await _powerShellScript.ExecuteAsync("./generate-api-client.ps1");

        if (result.IsFailure)
            throw new InvalidOperationException("Script failed");

        //await File.WriteAllTextAsync("./src/index.ts", "export { AppointmentsApiClient } from './AppointmentsApiClient", cancellationToken);
    }
}
