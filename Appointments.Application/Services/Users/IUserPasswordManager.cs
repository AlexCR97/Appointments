namespace Appointments.Application.Services.Users;

public interface IUserPasswordManager
{
    Task<string> GetAsync(Guid userId);
    Task<string> SetAsync(Guid userId, string password);
}
