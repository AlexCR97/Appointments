using Appointments.Api.Core.Models;
using Appointments.Api.Core.User;
using Appointments.Api.Tenant.Exceptions;
using Appointments.Api.Tenant.Models;
using Appointments.Common.Application;
using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
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
[AuthorizeTenant]
public class TenantsController : ControllerBase
{
    private readonly ISender _sender;

    public TenantsController(ISender sender)
    {
        _sender = sender;
    }

    #region Tenants

    [HttpGet("{tenantId}", Name = nameof(GetTenant))]
    public async Task<TenantProfileResponse> GetTenant([FromRoute] Guid tenantId)
    {
        var tenant = await _sender.Send(new Appointments.Core.Application.Requests.Tenants.GetTenantRequest(tenantId));
        return TenantProfileResponse.From(tenant);
    }

    [HttpPut("{tenantId}", Name = nameof(UpdateTenantProfile))]
    public async Task<NoContentResult> UpdateTenantProfile(
        [FromRoute] Guid tenantId,
        [FromBody] UpdateTenantProfileRequest request)
    {
        await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            tenantId));

        return NoContent();
    }

    #endregion

    #region Branch Offices

    [HttpPost("{tenantId}/branch-offices", Name = nameof(CreateBranchOffice))]
    public async Task<BranchOfficeCreatedResponse> CreateBranchOffice(
        [FromRoute] Guid tenantId,
        [FromBody] CreateBranchOfficeRequest request)
    {
        var branchOfficeId = await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            tenantId));

        return new BranchOfficeCreatedResponse(branchOfficeId);
    }

    [HttpGet("{tenantId}/branch-offices", Name = nameof(FindBranchOffices))]
    public async Task<PagedResult<BranchOfficeListResponse>> FindBranchOffices(
        [FromRoute] Guid tenantId,
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
                .And(@$"tenantId == ""{tenantId}""")
                .ToString()));

        return pagedResult.Map(BranchOfficeListResponse.From);
    }

    [HttpGet("{tenantId}/branch-offices/{branchOfficeId}", Name = nameof(GetBranchOffice))]
    public async Task<BranchOfficeProfileResponse> GetBranchOffice(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid branchOfficeId)
    {
        var branchOffice = await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.GetBranchOfficeRequest(branchOfficeId));

        if (branchOffice.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(BranchOffice), branchOfficeId.ToString());

        return BranchOfficeProfileResponse.From(branchOffice);
    }

    [HttpPut("{tenantId}/branch-offices/{branchOfficeId}", Name = nameof(UpdateBranchOffice))]
    public async Task<NoContentResult> UpdateBranchOffice(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid branchOfficeId,
        [FromBody] UpdateBranchOfficeRequest request)
    {
        var branchOffice = await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.GetBranchOfficeRequest(branchOfficeId));

        if (branchOffice.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(BranchOffice), branchOfficeId.ToString());

        await _sender.Send(request.ToApplicationRequest(
            branchOffice.Id,
            User.GetAccessToken().Username));

        return NoContent();
    }

    [HttpDelete("{tenantId}/branch-offices/{branchOfficeId}", Name = nameof(DeleteBranchOffice))]
    public async Task<NoContentResult> DeleteBranchOffice(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid branchOfficeId)
    {
        var branchOffice = await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.GetBranchOfficeRequest(branchOfficeId));

        if (branchOffice.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(BranchOffice), branchOfficeId.ToString());

        await _sender.Send(new Appointments.Core.Application.Requests.BranchOffices.DeleteBranchOfficeRequest(
            User.GetAccessToken().Username,
            branchOffice.Id));

        return NoContent();
    }

    #endregion

    #region Services

    [HttpPost("{tenantId}/services", Name = nameof(CreateService))]
    public async Task<ServiceCreatedResponse> CreateService(
        [FromRoute] Guid tenantId,
        [FromBody] CreateServiceRequest request)
    {
        var serviceId = await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            tenantId));

        return new ServiceCreatedResponse(serviceId);
    }

    [HttpGet("{tenantId}/services", Name = nameof(FindServices))]
    public async Task<PagedResult<ServiceListResponse>> FindServices(
        [FromRoute] Guid tenantId,
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
                .And(@$"tenantId == ""{tenantId}""")
                .ToString()));

        return pagedResult.Map(ServiceListResponse.From);
    }

    [HttpGet("{tenantId}/services/{serviceId}", Name = nameof(GetService))]
    public async Task<ServiceProfileResponse> GetService(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid serviceId)
    {
        var service = await _sender.Send(new Appointments.Core.Application.Requests.Services.GetServiceRequest(serviceId));

        if (service.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(Service), serviceId.ToString());

        return ServiceProfileResponse.From(service);
    }

    [HttpPut("{tenantId}/services/{serviceId}", Name = nameof(UpdateService))]
    public async Task<NoContentResult> UpdateService(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid serviceId,
        [FromBody] UpdateServiceRequest request)
    {
        var service = await _sender.Send(new Appointments.Core.Application.Requests.Services.GetServiceRequest(serviceId));

        if (service.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(Service), serviceId.ToString());

        await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            service.Id,
            service.TenantId));

        return NoContent();
    }

    [HttpDelete("{tenantId}/services/{serviceId}", Name = nameof(DeleteService))]
    public async Task<NoContentResult> DeleteService(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid serviceId)
    {
        var service = await _sender.Send(new Appointments.Core.Application.Requests.Services.GetServiceRequest(serviceId));

        if (service.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(Service), serviceId.ToString());

        await _sender.Send(new Appointments.Core.Application.Requests.Services.DeleteServiceRequest(
            User.GetAccessToken().Username,
            service.Id));

        return NoContent();
    }

    #endregion

    #region Appointments

    [HttpPost("{tenantId}/appointments", Name = nameof(CreateAppointment))]
    public async Task<AppointmentCreatedResponse> CreateAppointment(
        [FromRoute] Guid tenantId,
        [FromBody] CreateAppointmentRequest request)
    {
        var appointmentId = await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            tenantId));

        return new AppointmentCreatedResponse(appointmentId);
    }

    [HttpGet("{tenantId}/appointments", Name = nameof(FindAppointments))]
    public async Task<PagedResult<AppointmentListResponse>> FindAppointments(
        [FromRoute] Guid tenantId,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = FindRequest.PageSize.Default,
        [FromQuery] string? sort = null,
        [FromQuery] string? filter = null)
    {
        var pagedResult = await _sender.Send(new Appointments.Core.Application.Requests.Appointments.FindAppointmentsRequest(
            pageIndex,
            pageSize,
            sort,
            new FilterBuilder(filter)
                .And(@$"tenantId == ""{tenantId}""")
                .ToString()));

        return pagedResult.Map(AppointmentListResponse.From);
    }

    [HttpGet("{tenantId}/appointments/{appointmentId}", Name = nameof(GetAppointment))]
    public async Task<AppointmentProfileResponse> GetAppointment(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid appointmentId)
    {
        var appointment = await _sender.Send(new Appointments.Core.Application.Requests.Appointments.GetAppointmentRequest(appointmentId));

        if (appointment.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(Appointment), appointmentId.ToString());

        return AppointmentProfileResponse.From(appointment);
    }

    [HttpPut("{tenantId}/appointments/{appointmentId}/status", Name = nameof(SetAppointmentStatus))]
    public async Task<NoContentResult> SetAppointmentStatus(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid appointmentId,
        [FromBody] SetAppointmentStatusRequest request)
    {
        var appointment = await _sender.Send(new Appointments.Core.Application.Requests.Appointments.GetAppointmentRequest(appointmentId));

        if (appointment.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(Appointment), appointmentId.ToString());

        await _sender.Send(request.ToApplicationRequest(
            User.GetAccessToken().Username,
            appointment.Id));

        return NoContent();
    }

    [HttpDelete("{tenantId}/appointments/{appointmentId}", Name = nameof(DeleteAppointment))]
    public async Task<NoContentResult> DeleteAppointment(
        [FromRoute] Guid tenantId,
        [FromRoute] Guid appointmentId)
    {
        var appointment = await _sender.Send(new Appointments.Core.Application.Requests.Appointments.GetAppointmentRequest(appointmentId));

        if (appointment.TenantId != tenantId)
            throw new OwnershipException("Tenant", tenantId.ToString(), nameof(Appointment), appointmentId.ToString());

        await _sender.Send(new Appointments.Core.Application.Requests.Appointments.DeleteAppointmentRequest(
            User.GetAccessToken().Username,
            appointment.Id));

        return NoContent();
    }

    #endregion
}
