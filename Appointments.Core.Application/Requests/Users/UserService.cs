using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Application.Requests.Users;

public interface IUserService
{
    Task<User> LoginWithEmailAsync(Email email, string password);
}

internal sealed class UserService : IUserService
{
    private readonly IUserPasswordManager _userPasswordManager;
    private readonly IUserRepository _userRepository;

    public UserService(IUserPasswordManager userPasswordManager, IUserRepository userRepository)
    {
        _userPasswordManager = userPasswordManager;
        _userRepository = userRepository;
    }

    public async Task<User> LoginWithEmailAsync(Email email, string password)
    {
        var user = await _userRepository.GetByEmailOrDefaultAsync(email)
            ?? throw new InvalidCredentialsException();

        var isPasswordValid = await IsPasswordValid(password, user.GetLocalLogin());

        if (!isPasswordValid)
            throw new InvalidCredentialsException();

        return user;
    }

    private async Task<bool> IsPasswordValid(string providedPassword, LocalLogin login)
    {
        var actualPassword = await _userPasswordManager.GetAsync(login.Password);
        return providedPassword == actualPassword;
    }
}
