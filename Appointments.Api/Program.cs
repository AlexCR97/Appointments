using Appointments.Api.Connect.DependencyInjection;
using Appointments.Api.Filters.Exceptions;
using Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;
using Appointments.Api.Tenant.DependencyInjection;
using Appointments.Assets.DependencyInjection;
using Appointments.Core.Application.DependencyInjection;
using Appointments.Core.DependencyInjection;
using Appointments.Core.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

const string DefaultCorsPolicy = "DefaultCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(config =>
    {
        config.Filters.Add(typeof(ExceptionFilter));
    })
    .AddTenantApi()
    .AddConnectApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy(DefaultCorsPolicy, policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetRequiredSection("Cors")
            .GetRequiredSection("Default")
            .GetRequiredSection("AllowedOrigins")
            .Get<string[]>()
            ?? Array.Empty<string>();

        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            //ValidateAudience = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetRequiredString("Jwt:Issuer"),
            //ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            ValidAudience = null,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration.GetRequiredString("Jwt:SecretKey"))),
        };
    });

builder.Services.AddAuthorization(config =>
{
    config.AddTenantApiPolicies();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services
    .AddHttpContextAccessor()
    .AddScoped<IProblemDetailsFactory<Exception>, ExceptionProblemDetailsFactory>();

builder.Services
    .AddCoreModule(builder.Configuration)
    // TODO Enable Assets module
    //.AddAssetsModule(builder.Configuration)
    ;

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(DefaultCorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
