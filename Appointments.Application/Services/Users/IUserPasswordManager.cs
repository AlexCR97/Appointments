namespace Appointments.Application.Services.Users;

public interface IUserPasswordManager
{
    Task<string> SaveAsync(string plainTextPassword);
}
