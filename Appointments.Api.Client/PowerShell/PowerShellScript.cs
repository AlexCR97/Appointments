using Appointments.Common.Domain.Models;
using System.Management.Automation;

namespace Appointments.Api.Client.PowerShell;

internal interface IPowerShellScript
{
    Task<Result<PowerShellException>> ExecuteAsync(string scriptPath, Dictionary<string, object>? parameters = null);
}

internal sealed class PowerShellScript : IPowerShellScript
{
    private readonly IPowerShellRunSpace _powerShellRunSpace;

    private readonly List<string> _errorMessages = new();

    public PowerShellScript(IPowerShellRunSpace powerShellRunSpace)
    {
        _powerShellRunSpace = powerShellRunSpace;
    }

    public async Task<Result<PowerShellException>> ExecuteAsync(string scriptPath, Dictionary<string, object>? parameters = null)
    {
        try
        {
            _errorMessages.Clear();

            if (!_powerShellRunSpace.IsInitialized)
                _powerShellRunSpace.Initialize();

            using var ps = System.Management.Automation.PowerShell.Create();
            ps.RunspacePool = _powerShellRunSpace.RunSpacePool;
            ps.AddCommand(scriptPath);

            if (parameters is not null)
                foreach (var scriptParameter in parameters)
                    ps.AddParameter(scriptParameter.Key, scriptParameter.Value);

            ps.Streams.Debug.DataAdded += (sender, e) =>
            {
            };

            ps.Streams.Error.DataAdded += Error_DataAdded;

            ps.Streams.Information.DataAdded += Information_DataAdded;

            ps.Streams.Progress.DataAdded += (sender, e) =>
            {
            };

            ps.Streams.Verbose.DataAdded += (sender, e) =>
            {
            };

            ps.Streams.Warning.DataAdded += Warning_DataAdded;

            var output = await ps.InvokeAsync();

            foreach (var psObject in output)
            {
            }

            if (_errorMessages.Any())
            {
                var powerShellException = new PowerShellException(
                    "Script failed with errors. See exception data for more details.")
                    .WithData("Errors", _errorMessages.AsReadOnly());

                return Result<PowerShellException>.Failure(powerShellException);
            }

            return Result<PowerShellException>.Success();
        }
        catch (Exception ex)
        {
            return Result<PowerShellException>.Failure(new PowerShellException(
                "An unexpected error occurred. See inner exception for more details.",
                ex));
        }
    }

    private void Error_DataAdded(object? sender, DataAddedEventArgs e)
    {
        if (sender is not PSDataCollection<ErrorRecord> streamObjectsReceived)
            return;

        var currentStreamRecord = streamObjectsReceived[e.Index];

        _errorMessages.Add(currentStreamRecord.Exception.Message);
    }

    private void Warning_DataAdded(object? sender, DataAddedEventArgs e)
    {
        if (sender is not PSDataCollection<WarningRecord> streamObjectsReceived)
            return;

        var currentStreamRecord = streamObjectsReceived[e.Index];
    }

    private void Information_DataAdded(object? sender, DataAddedEventArgs e)
    {
        if (sender is not PSDataCollection<InformationRecord> streamObjectsReceived)
            return;

        var currentStreamRecord = streamObjectsReceived[e.Index];
    }
}
