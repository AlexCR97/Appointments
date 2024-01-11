using Appointments.Api.Assets.DependencyInjection;
using Appointments.Api.Connect.DependencyInjection;
using Appointments.Api.Core.DependencyInjection;
using Appointments.Api.Filters.Exceptions;
using Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;
using Appointments.Api.Tenant.DependencyInjection;
using Appointments.Assets.DependencyInjection;
using Appointments.Core.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(config => config.Filters.Add(typeof(ExceptionFilter)))
    .AddConnectApi()
    .AddTenantApi()
    .AddAssetsApi();

builder.Services.AddAuthorization(config => config
    .AddTenantApiPolicies()
    .AddAssetsApiPolicies());

builder.Services
    .AddCore(builder.Configuration)
    .AddCoreModule(builder.Configuration)
    .AddAssetsModule(builder.Configuration);

builder.Services.AddScoped<IProblemDetailsFactory<Exception>, ExceptionProblemDetailsFactory>();

var app = builder.Build();

app.UseCore();

app.Run();
