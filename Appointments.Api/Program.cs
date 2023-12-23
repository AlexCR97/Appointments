using Appointments.Api.Filters.Exceptions;
using Appointments.Api.Filters.Exceptions.ProblemDetailsFactories;
using Appointments.Api.Management;
using Appointments.Application.DependencyInjection;
using Appointments.Application.Policies;
using Appointments.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
    config
        .AddPolicy(UserPolicy.PolicyName, policy => policy
            .RequireRole(new string[]
            {
                UserPolicy.Roles.Owner,
                UserPolicy.Roles.Admin,
                UserPolicy.Roles.Writer,
                UserPolicy.Roles.Reader,
            }));

    config
        .AddPolicy(TenantPolicy.PolicyName, policy => policy
            .RequireRole(new string[]
            {
                TenantPolicy.Roles.Owner,
                TenantPolicy.Roles.Admin,
                TenantPolicy.Roles.Writer,
                TenantPolicy.Roles.Reader,
            }));

    config
        .AddPolicy(ServicePolicy.PolicyName, policy => policy
            .RequireRole(new string[]
            {
                ServicePolicy.Roles.Owner,
                ServicePolicy.Roles.Admin,
                ServicePolicy.Roles.Writer,
                ServicePolicy.Roles.Reader,
            }));

    config
        .AddPolicy(BranchOfficePolicy.PolicyName, policy => policy
            .RequireRole(new string[]
            {
                BranchOfficePolicy.Roles.Owner,
                BranchOfficePolicy.Roles.Admin,
                BranchOfficePolicy.Roles.Writer,
                BranchOfficePolicy.Roles.Reader,
            }));
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapManagementApi();

app.Run();
