namespace Appointments.Jobs.Infrastructure;

internal sealed class LuceneQueryBuilder
{
    private readonly List<string> _queryParts = new();

    public LuceneQueryBuilder And(string? query)
    {
        if (query is not null)
            _queryParts.Add($"({query})");

        return this;
    }

    public string? Build()
    {
        if (_queryParts.Count == 0)
            return null;

        return string.Join(" AND ", _queryParts);
    }
}
