using Appointments.Application.Requests.Tenants;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Management;

internal static class ManagementApi
{
    public static IEndpointRouteBuilder MapManagementApi(this IEndpointRouteBuilder builder)
    {
        return builder
            .MapGroup("/management")
            .MapTenantsApi()
            .MapUsersApi();
    }

    private static RouteGroupBuilder MapTenantsApi(this RouteGroupBuilder builder)
    {
        builder
            .MapGroup("/tenants")
            .MapPost("/", (
                [FromServices] ISender sender,
                [FromBody] CreateTenantRequest request
                ) =>
            {
            });

        return builder;
    }

    private static RouteGroupBuilder MapUsersApi(this RouteGroupBuilder builder)
    {
        builder
            .MapGroup("/users");

        return builder;
    }
}
