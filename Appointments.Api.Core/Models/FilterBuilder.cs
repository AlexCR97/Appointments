namespace Appointments.Api.Core.Models;

public sealed class FilterBuilder
{
    private readonly List<string> _filterParts = new();

    public FilterBuilder(string? filter)
    {
        TryAddFilterPart(filter);
    }

    public FilterBuilder And(string? filter)
    {
        TryAddFilterPart(filter);
        return this;
    }

    private void TryAddFilterPart(string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return;

        _filterParts.Add($"({filter})");
    }

    public override string ToString()
    {
        return string.Join(" and ", _filterParts);
    }
}
