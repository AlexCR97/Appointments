using Appointments.Application.Exceptions;
using Appointments.Application.Requests.BranchOffices;
using Appointments.Application.Requests.Services;
using Appointments.Application.Requests.Tenants;
using Appointments.Application.Services.Events;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Users;

public sealed record SignUpWithEmailRequest(
    string FirstName,
    string LastName,
    Email Email,
    string Password,
    string PasswordConfirm,
    string CompanyName)
    : IRequest<UserSignedUpResult>;

public sealed record UserSignedUpResult(
    Guid UserId,
    Guid TenantId);

internal sealed class SignUpWithEmailRequestValidator : AbstractValidator<SignUpWithEmailRequest>
{
    public SignUpWithEmailRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .SetValidator(new EmailValidator());

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty()
            .Must((request, passwordConfirm) => passwordConfirm == request.Password);

        RuleFor(x => x.CompanyName)
            .NotEmpty();
    }
}

internal sealed class SignUpWithEmailRequestHandler : IRequestHandler<SignUpWithEmailRequest, UserSignedUpResult>
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

    public async Task<UserSignedUpResult> Handle(SignUpWithEmailRequest request, CancellationToken cancellationToken)
    {
        new SignUpWithEmailRequestValidator().ValidateAndThrow(request);

        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new AlreadyExistsException<User>("Email", request.Email);

        var tenant = await CreateTenantAsync(request.CompanyName);
        var user = await CreateUserAsync(request.FirstName, request.LastName, request.Email, request.PasswordConfirm, tenant);
        await CreateBranchOfficeAsync(tenant);
        await CreateServiceAsync(tenant);

        return new UserSignedUpResult(user.Id, tenant.Id);
    }

    private async Task<Tenant> CreateTenantAsync(string name)
    {
        var tenant = Tenant.Create(
            Constants.CreatedBy.EmailSignUp,
            name,
            null,
            TenantUrlId.Random(),
            null,
            new List<SocialMediaContact>(),
            null);

        if (tenant.HasChanged)
        {
            await _tenantRepository.CreateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }

        return tenant;
    }

    private async Task<User> CreateUserAsync(
        string firstName,
        string lastName,
        Email email,
        string password,
        Tenant tenant)
    {
        var user = User.CreateWithLocalLogin(
            Constants.CreatedBy.EmailSignUp,
            firstName,
            lastName,
            email,
            password,
            new UserTenant(tenant.Id, tenant.Name));

        if (user.HasChanged)
        {
            await _userRepository.CreateAsync(user);
            await _eventProcessor.ProcessAsync(user.Events);
        }

        return user;
    }

    private async Task CreateBranchOfficeAsync(Tenant tenant)
    {
        var branchOffice = BranchOffice.Create(
            Constants.CreatedBy.EmailSignUp,
            tenant.Id,
            "Default",
            Address.Default(),
            new List<SocialMediaContact>(),
            null);

        if (branchOffice.HasChanged)
        {
            await _branchOfficeRepository.CreateAsync(branchOffice);
            await _eventProcessor.ProcessAsync(branchOffice.Events);
        }
    }

    private async Task CreateServiceAsync(Tenant tenant)
    {
        var service = Service.Create(
            Constants.CreatedBy.EmailSignUp,
            tenant.Id,
            "Default",
            null,
            0,
            new List<IndexedResource>(),
            new List<string>(),
            null,
            null,
            null);

        if (service.HasChanged)
        {
            await _serviceRepository.CreateAsync(service);
            await _eventProcessor.ProcessAsync(service.Events);
        }
    }
}
