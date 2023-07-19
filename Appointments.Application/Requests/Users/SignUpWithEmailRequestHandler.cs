using Appointments.Application.Exceptions;
using Appointments.Application.Extensions.Normalization;
using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Repositories.Users;
using Appointments.Application.Services.Events;
using Appointments.Application.Services.Users;
using Appointments.Domain.Entities;
using MediatR;
using PasswordGenerator;

namespace Appointments.Application.Requests.Users;

internal class SignUpWithEmailRequestHandler : IRequestHandler<SignUpWithEmailRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserPasswordManager _userPasswordManager;
    private readonly IUserRepository _userRepository;

    public SignUpWithEmailRequestHandler(IEventProcessor eventProcessor, ITenantRepository tenantRepository, IUserPasswordManager userPasswordManager, IUserRepository userRepository)
    {
        _eventProcessor = eventProcessor;
        _tenantRepository = tenantRepository;
        _userPasswordManager = userPasswordManager;
        _userRepository = userRepository;
    }

    public async Task Handle(SignUpWithEmailRequest request, CancellationToken cancellationToken)
    {
        var createdUser = await CreateUserAsync(request);
        var createdTenant = await CreateTenantAsync(request, createdUser);
        await SetSelectedTenantAsync(createdUser, createdTenant);
    }

    private async Task<User> CreateUserAsync(SignUpWithEmailRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new AlreadyExistsException<User>(nameof(User.Email), request.Email);

        var managedPassword = await _userPasswordManager.SaveAsync(request.Password);

        var user = User.CreateWithEmailCredentials(
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
        var tenant = new Tenant(
            user.Email,
            request.TenantName,
            null,
            new Password()
                .LengthRequired(Tenant.UrlIdLength)
                .IncludeUppercase()
                .IncludeLowercase()
                .IncludeNumeric()
                .Next(),
            null,
            null,
            null);

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
        var branchOffice = BranchOffice.CreateSlim(
            user.Email,
            tenant.Id);

        // TODO Implement
    }
}
