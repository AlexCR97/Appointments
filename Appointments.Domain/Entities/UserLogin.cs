using Appointments.Common.Domain.Models;
using Appointments.Common.Utils.Security;

namespace Appointments.Core.Domain.Entities;

public interface IUserLogin
{
    IdentityProvider IdentityProvider { get; }
    bool Confirmed { get; }
    public DateTime? ConfirmedAt { get; }

    void Confirm();
}

public enum IdentityProvider
{
    Local,
}

public abstract class UserLogin : IUserLogin
{
    public IdentityProvider IdentityProvider { get; }
    public bool Confirmed { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }

    protected UserLogin(IdentityProvider identityProvider, bool confirmed, DateTime? confirmedAt)
    {
        IdentityProvider = identityProvider;
        Confirmed = confirmed;
        ConfirmedAt = confirmedAt;
    }

    public void Confirm()
    {
        Confirmed = true;
        ConfirmedAt = DateTime.UtcNow;
    }
}

public sealed class LocalLogin : UserLogin
{
    public Email Email { get; }
    public string Password { get; }
    public string ConfirmationCode { get; }
    public DateTime ConfirmationCodeExpiration { get; }

    public LocalLogin(bool confirmed, DateTime? confirmedAt, Email email, string password, string confirmationCode, DateTime confirmationCodeExpiration)
        : base(IdentityProvider.Local, confirmed, confirmedAt)
    {
        Email = email;
        Password = password;
        ConfirmationCode = confirmationCode;
        ConfirmationCodeExpiration = confirmationCodeExpiration;
    }

    public static LocalLogin Create(Email email, string password)
    {
        return new LocalLogin(
            false,
            null,
            email,
            password,
            KeyGenerator.Random(48),
            DateTime.UtcNow.AddDays(2));
    }
}
