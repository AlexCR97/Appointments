using System.Management.Automation.Runspaces;

namespace Appointments.Api.Client.PowerShell;

internal interface IPowerShellRunSpace
{
    public RunspacePool RunSpacePool { get; }
    public bool IsInitialized { get; }
    void Initialize();
}

internal sealed class PowerShellRunSpace : IPowerShellRunSpace
{
    private readonly PowerShellOptions _options;

    public PowerShellRunSpace(PowerShellOptions options)
    {
        _options = options;
    }

    private RunspacePool? _runSpacePool;
    public RunspacePool RunSpacePool => _runSpacePool
        ?? throw new PowerShellException("RunSpacePool has not been initialized");

    public bool IsInitialized => _runSpacePool is not null;

    public void Initialize()
    {
        if (IsInitialized)
            throw new PowerShellException("RunSpacePool has already been initialized");

        var sessionState = InitialSessionState.CreateDefault();
        sessionState.ThrowOnRunspaceOpenError = true;
        sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

        if (_options.Runspace.Modules is not null && _options.Runspace.Modules.Any())
            sessionState.ImportPSModule(_options.Runspace.Modules);

        _runSpacePool = RunspaceFactory.CreateRunspacePool(sessionState);
        _runSpacePool.SetMinRunspaces(_options.Runspace.MinRunspaces);
        _runSpacePool.SetMaxRunspaces(_options.Runspace.MaxRunspaces);
        _runSpacePool.ThreadOptions = PSThreadOptions.UseNewThread;
        _runSpacePool.Open();
    }
}
