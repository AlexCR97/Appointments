using Appointments.Api.Core.Models;
using Appointments.Api.Core.User;
using Appointments.Api.Tenant.Exceptions;
using Appointments.Api.Tenant.Models;
using Appointments.Common.Application;
using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Core.Application.Requests.Tenants;
using Appointments.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Tenant.Controllers;

[ApiController]
[Route("api/tenant/tenants")]
[ApiVersion("1.0")]
[Produces("application/json")]
[Authorize(Policy = TenantApiPolicy.Tenants.Scope)]
public class TenantsController : ControllerBase
{
    private readonly ISender _sender;

    public TenantsController(ISender sender)
    {
        _sender = sender;
    }

    #region Tenants

    [HttpGet("{id}", Name = nameof(GetTenant))]
    public async Task<TenantProfileResponse> GetTenant([FromRoute] Guid id)
    {
        var tenant = await _sender.Send(new GetTenantRequest(id));
        return TenantProfileResponse.From(tenant);
    }

    [HttpPut("{id}", Name = nameof(UpdateTenantProfile))]
    public async Task<NoContentResult> UpdateTenantProfile(
        [FromRoute] Guid id,
        [FromBody] UpdateTenantProfileRequest request)
    {
        IdMismatchException.ThrowIfMismatch(id.ToString(), request.Id.ToString());
        await _sender.Send(request);
        return NoContent();
    }

    #endregion

    #region Branch Offices

    [HttpPost("{id}/branch-offices", Name = nameof(CreateBranchOffice))]
    public async Task<BranchOfficeCreatedResponse> CreateBranchOffice(
        [FromRoute] Guid id,
        [FromBody] CreateBranchOfficeRequest request)
    {
        var branchOfficeId = await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            id));

        return new BranchOfficeCreatedResponse(branchOfficeId);
    }

    [HttpGet("{id}/branch-offices", Name = nameof(FindBranchOffices))]
    public async Task<PagedResult<BranchOfficeListResponse>> FindBranchOffices(
        [FromRoute] Guid id,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = FindRequest.PageSize.Default,
        [FromQuery] string? sort = null,
        [FromQuery] string? filter = null)
    {
        var pagedResult = await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.FindBranchOfficesRequest(
            pageIndex,
            pageSize,
            sort,
            new FilterBuilder(filter)
                .And(@$"tenantId == ""{id}""")
                .ToString()));

        return pagedResult.Map(BranchOfficeListResponse.From);
    }

    [HttpGet("{id}/branch-offices/{branchOfficeId}", Name = nameof(GetBranchOffice))]
    public async Task<BranchOfficeProfileResponse> GetBranchOffice(
        [FromRoute] Guid id,
        [FromRoute] Guid branchOfficeId)
    {
        var branchOffice = await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.GetBranchOfficeRequest(branchOfficeId));

        if (branchOffice.TenantId != id)
            throw new OwnershipException("Tenant", id.ToString(), nameof(BranchOffice), branchOfficeId.ToString());

        return BranchOfficeProfileResponse.From(branchOffice);
    }

    [HttpPut("{id}/branch-offices/{branchOfficeId}", Name = nameof(UpdateBranchOffice))]
    public async Task<NoContentResult> UpdateBranchOffice(
        [FromRoute] Guid id,
        [FromRoute] Guid branchOfficeId,
        [FromBody] UpdateBranchOfficeRequest request)
    {
        var branchOffice = await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.GetBranchOfficeRequest(branchOfficeId));

        if (branchOffice.TenantId != id)
            throw new OwnershipException("Tenant", id.ToString(), nameof(BranchOffice), branchOfficeId.ToString());

        await _sender.Send(request.ToApplicationRequest(
            branchOffice.Id,
            User.GetAccessToken().Username));

        return NoContent();
    }

    [HttpDelete("{id}/branch-offices/{branchOfficeId}", Name = nameof(DeleteBranchOffice))]
    public async Task<NoContentResult> DeleteBranchOffice(
        [FromRoute] Guid id,
        [FromRoute] Guid branchOfficeId)
    {
        var branchOffice = await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.GetBranchOfficeRequest(branchOfficeId));

        if (branchOffice.TenantId != id)
            throw new OwnershipException("Tenant", id.ToString(), nameof(BranchOffice), branchOfficeId.ToString());

        await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.DeleteBranchOfficeRequest(
            User.GetAccessToken().Username,
            branchOffice.Id));

        return NoContent();
    }

    #endregion

    #region Services

    [HttpPost("{id}/services", Name = nameof(CreateService))]
    public async Task<ServiceCreatedResponse> CreateService(
        [FromRoute] Guid id,
        [FromBody] CreateServiceRequest request)
    {
        var serviceId = await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            id));

        return new ServiceCreatedResponse(serviceId);
    }

    [HttpGet("{id}/services", Name = nameof(FindServices))]
    public async Task<PagedResult<ServiceListResponse>> FindServices(
        [FromRoute] Guid id,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = FindRequest.PageSize.Default,
        [FromQuery] string? sort = null,
        [FromQuery] string? filter = null)
    {
        var pagedResult = await _sender.Send(new Appointments.Core.Application.Requests.Services.FindServicesRequest(
            pageIndex,
            pageSize,
            sort,
            new FilterBuilder(filter)
                .And(@$"tenantId == ""{id}""")
                .ToString()));

        return pagedResult.Map(ServiceListResponse.From);
    }

    [HttpGet("{id}/services/{serviceId}", Name = nameof(GetService))]
    public async Task<ServiceProfileResponse> GetService(
        [FromRoute] Guid id,
        [FromRoute] Guid serviceId)
    {
        var service = await _sender.Send(new Appointments.Core.Application.Requests.Services.GetServiceRequest(serviceId));

        if (service.TenantId != id)
            throw new OwnershipException("Tenant", id.ToString(), nameof(Service), serviceId.ToString());

        return ServiceProfileResponse.From(service);
    }

    [HttpPut("{id}/services/{serviceId}", Name = nameof(UpdateService))]
    public async Task<NoContentResult> UpdateService(
        [FromRoute] Guid id,
        [FromRoute] Guid serviceId,
        [FromBody] UpdateServiceRequest request)
    {
        var service = await _sender.Send(new Appointments.Core.Application.Requests.Services.GetServiceRequest(serviceId));

        if (service.TenantId != id)
            throw new OwnershipException("Tenant", id.ToString(), nameof(Service), serviceId.ToString());

        await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            service.Id,
            service.TenantId));

        return NoContent();
    }

    [HttpDelete("{id}/services/{serviceId}", Name = nameof(DeleteService))]
    public async Task<NoContentResult> DeleteService(
        [FromRoute] Guid id,
        [FromRoute] Guid serviceId)
    {
        var service = await _sender.Send(new Appointments.Core.Application.Requests.Services.GetServiceRequest(serviceId));

        if (service.TenantId != id)
            throw new OwnershipException("Tenant", id.ToString(), nameof(Service), serviceId.ToString());

        await _sender.Send(new Appointments.Core.Application.Requests.Services.DeleteServiceRequest(
            User.GetAccessToken().Username,
            service.Id));

        return NoContent();
    }

    #endregion
}
