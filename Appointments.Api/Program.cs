using Appointments.Api.Filters.Exceptions;
using Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;
using Appointments.Application.DependencyInjection;
using Appointments.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config =>
{
    config.Filters.Add(typeof(ExceptionFilter));
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

app.UseAuthorization();

app.MapControllers();

app.Run();
