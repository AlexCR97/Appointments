namespace Appointments.Core.Application.Requests.Users;

public interface IUserPasswordManager
{
    Task<string> GetAsync(string email);
    Task<string> SetAsync(string email, string password);
}
