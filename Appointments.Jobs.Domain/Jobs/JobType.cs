namespace Appointments.Jobs.Domain.Jobs;

public enum JobType
{
    Unknown,

    /// <summary>
    /// Checks user login methods that have not been confirmed
    /// and sends an email to remember confirmation is pending.
    /// </summary>
    LoginMethodConfirmationReminder,
}
