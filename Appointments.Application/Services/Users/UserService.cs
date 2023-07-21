using Appointments.Application.Exceptions;
using Appointments.Application.Repositories.Users;
using Appointments.Domain.Entities;

namespace Appointments.Application.Services.Users;

public interface IUserService
{
    Task<User> LoginAsync(string email, string password);
}

internal class UserService : IUserService
{
    private readonly IUserPasswordManager _userPasswordManager;
    private readonly IUserRepository _userRepository;

    public UserService(IUserPasswordManager userPasswordManager, IUserRepository userRepository)
    {
        _userPasswordManager = userPasswordManager;
        _userRepository = userRepository;
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailOrDefaultAsync(email)
            ?? throw new InvalidCredentialsException();

        var isPasswordValid = await IsPasswordValid(password, user);

        if (!isPasswordValid)
            throw new InvalidCredentialsException();

        return user;
    }

    private async Task<bool> IsPasswordValid(string providedPassword, User user)
    {
        if (user.IsPasswordPlainText)
            return providedPassword == user.Password;

        var managedPassword = await _userPasswordManager.GetAsync(user.Id);

        return providedPassword == managedPassword;
    }
}
