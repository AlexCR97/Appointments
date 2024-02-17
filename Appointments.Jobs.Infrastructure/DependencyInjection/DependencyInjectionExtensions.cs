using Appointments.Common.MongoClient.DependencyInjection;
using Appointments.Jobs.Application.UseCases.Executions;
using Appointments.Jobs.Application.UseCases.Jobs;
using Appointments.Jobs.Application.UseCases.Triggers;
using Appointments.Jobs.Infrastructure.Mongo.Executions;
using Appointments.Jobs.Infrastructure.Mongo.Jobs;
using Appointments.Jobs.Infrastructure.Mongo.Triggers;
using Appointments.Jobs.Infrastructure.UseCases.Executions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;

namespace Appointments.Jobs.Infrastructure.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddCustomQuartz()
            .AddMongo();
    }

    private static IServiceCollection AddCustomQuartz(this IServiceCollection services)
    {
        return services
            .AddQuartz(config =>
            {
                config.UseInMemoryStore();
            })
            .AddQuartzServer(options =>
            {
            });
    }

    private static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services
            .AddMongoRepository<JobDocument>(JobDocument.CollectionName);

        services
            .AddScoped<IExecutionRepository, ExecutionRepository>()
            .AddScoped<IJobRepository, JobRepository>()
            .AddScoped<ITriggerRepository, TriggerRepository>();

        return services;
    }

    public static IBusRegistrationConfigurator AddJobsInfrastructure(this IBusRegistrationConfigurator config)
    {
        config.AddConsumer<ExecutionQueuedConsumer>();
        return config;
    }
}
