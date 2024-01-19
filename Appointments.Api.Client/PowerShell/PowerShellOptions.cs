namespace Appointments.Api.Client.PowerShell;

public sealed class PowerShellOptions
{
    public const string SectionName = "PowerShell";

    public PowerShellRunspaceOptions Runspace { get; init; } = new();
}

public sealed class PowerShellRunspaceOptions
{
    public int MinRunspaces { get; init; }
    public int MaxRunspaces { get; init; }
    public string[] Modules { get; init; } = default!;
}
