using Appointments.Api.Assets.DependencyInjection;
using Appointments.Api.Client;
using Appointments.Api.Client.PowerShell.DependencyInjection;
using Appointments.Api.Connect.DependencyInjection;
using Appointments.Api.Core.DependencyInjection;
using Appointments.Api.Tenant.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddConnectApi()
    .AddTenantApi()
    .AddAssetsApi();

builder.Services.AddAuthorization(config => config
    .AddTenantApiPolicies()
    .AddAssetsApiPolicies());

builder.Services
    .AddCore(builder.Configuration);

builder.Services
    .AddPowerShell(builder.Configuration)
    .AddSingleton<IApiClientGenerator, ApiClientGenerator>();

var app = builder.Build();

app.UseCore();

app.Services
    .GetRequiredService<IApiClientGenerator>()
    .GenerateAsync()
    .Wait();

app.StopAsync().Wait();
