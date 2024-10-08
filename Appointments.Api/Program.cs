using Appointments.Api.Assets.DependencyInjection;
using Appointments.Api.Connect.DependencyInjection;
using Appointments.Api.Core.DependencyInjection;
using Appointments.Api.Filters.Exceptions;
using Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;
using Appointments.Api.Management.DependencyInjection;
using Appointments.Api.Tenant.DependencyInjection;
using Appointments.Assets.DependencyInjection;
using Appointments.Core.DependencyInjection;
using Appointments.Infrastructure.DependencyInjection;
using Appointments.Jobs.DependencyInjection;
using Appointments.Notifications.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseLogging();

builder.Services
    .AddControllers(config => config.Filters.Add(typeof(ExceptionFilter)))
    .AddConnectApi()
    .AddTenantApi()
    .AddAssetsApi()
    .AddManagementApi();

builder.Services.AddAuthorization(config => config
    .AddTenantApiPolicies()
    .AddAssetsApiPolicies()
    .AddManagementApiPolicies());

builder.Services
    .AddCore(builder.Configuration)
    .AddCoreModule(builder.Configuration)
    .AddAssetsModule(builder.Configuration)
    .AddJobsModule()
    .AddNotificationsModule(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IProblemDetailsFactory<Exception>, ExceptionProblemDetailsFactory>();

var app = builder.Build();

app.UseCore();

app.Run();

// For integration testing
public partial class Program { }
