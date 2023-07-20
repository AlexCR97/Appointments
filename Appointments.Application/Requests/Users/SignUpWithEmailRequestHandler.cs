using Appointments.Application.Exceptions;
using Appointments.Application.Extensions.Normalization;
using Appointments.Application.Repositories.BranchOffices;
using Appointments.Application.Repositories.Services;
using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Repositories.Users;
using Appointments.Application.Services.Events;
using Appointments.Application.Services.Users;
using Appointments.Application.Validations.Users;
using Appointments.Domain.Entities;
using FluentValidation;
using MediatR;
using PasswordGenerator;

namespace Appointments.Application.Requests.Users;

internal class SignUpWithEmailRequestHandler : IRequestHandler<SignUpWithEmailRequest, Guid>
{
    private readonly IBranchOfficeRepository _branchOfficeRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly IServiceRepository _serviceRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserPasswordManager _userPasswordManager;
    private readonly IUserRepository _userRepository;

    public SignUpWithEmailRequestHandler(IBranchOfficeRepository branchOfficeRepository, IEventProcessor eventProcessor, IServiceRepository serviceRepository, ITenantRepository tenantRepository, IUserPasswordManager userPasswordManager, IUserRepository userRepository)
    {
        _branchOfficeRepository = branchOfficeRepository;
        _eventProcessor = eventProcessor;
        _serviceRepository = serviceRepository;
        _tenantRepository = tenantRepository;
        _userPasswordManager = userPasswordManager;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(SignUpWithEmailRequest request, CancellationToken cancellationToken)
    {
        new SignUpWithEmailRequestValidator().ValidateAndThrow(request);

        var createdUser = await CreateUserAsync(request);
        var createdTenant = await CreateTenantAsync(request, createdUser);
        await SetSelectedTenantAsync(createdUser, createdTenant);
        await CreateBranchOfficeAsync(createdUser, createdTenant);
        await CreateServiceAsync(createdUser, createdTenant);
        return createdUser.Id;
    }

    private async Task<User> CreateUserAsync(SignUpWithEmailRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new AlreadyExistsException<User>(nameof(User.Email), request.Email);

        var userId = Guid.NewGuid();

        var managedPassword = await _userPasswordManager.SetAsync(userId, request.Password);

        var user = User.CreateWithEmailCredentials(
            userId,
            null,
            request.Email.NormalizeForComparison(),
            managedPassword,
            false,
            request.FirstName.Trim(),
            request.LastName.Trim(),
            null);

        if (user.HasChanged)
        {
            await _userRepository.CreateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }

        return user;
    }

    private async Task<Tenant> CreateTenantAsync(SignUpWithEmailRequest request, User user)
    {
        var tenant = Tenant.CreateMinimal(
            user.Email,
            request.TenantName,
            new Password()
                .LengthRequired(Tenant.UrlIdLength)
                .IncludeUppercase()
                .IncludeLowercase()
                .IncludeNumeric()
                .Next());

        if (tenant.HasChanged)
        {
            await _tenantRepository.CreateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }

        return tenant;
    }

    private async Task SetSelectedTenantAsync(User user, Tenant tenant)
    {
        user.SetSelectedTenant(tenant, user.Email);

        if (user.HasChanged)
        {
            await _userRepository.UpdateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }
    }

    private async Task CreateBranchOfficeAsync(User user, Tenant tenant)
    {
        var branchOffice = BranchOffice.CreateMinimal(
            user.Email,
            tenant.Id);

        if (branchOffice.HasChanged)
        {
            await _branchOfficeRepository.CreateAsync(branchOffice);
            await _eventProcessor.ProcessAsync(branchOffice.Events);
        }
    }

    private async Task CreateServiceAsync(User user, Tenant tenant)
    {
        var service = Service.CreateMinimal(
            user.Email,
            tenant.Id);

        if (service.HasChanged)
        {
            await _serviceRepository.CreateAsync(service);
            await _eventProcessor.ProcessAsync(service.Events);
        }
    }
}
