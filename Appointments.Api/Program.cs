using Appointments.Api.Filters.Exceptions;
using Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;
using Appointments.Application.DependencyInjection;
using Appointments.Infrastructure.DependencyInjection;

const string DefaultCorsPolicy = "DefaultCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config =>
{
    config.Filters.Add(typeof(ExceptionFilter));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(DefaultCorsPolicy, policy =>
    {
        var whitelist = builder.Configuration
            .GetRequiredSection("Cors")
            .GetRequiredSection("Default")
            .Get<string[]>()
            ?? Array.Empty<string>();

        policy
            .WithOrigins(whitelist)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHttpContextAccessor()
    .AddScoped<IProblemDetailsFactory<Exception>, ExceptionProblemDetailsFactory>()
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(DefaultCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
