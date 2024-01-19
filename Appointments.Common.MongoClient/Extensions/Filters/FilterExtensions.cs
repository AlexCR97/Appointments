using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Appointments.Common.MongoClient.Extensions.Filters;

internal static class FilterExtensions
{
    public static Expression<Func<T, bool>> ToExpression<T>(
        this string filter,
        IReadOnlyCollection<object>? filterParams = null)
    {
        var expression = DynamicExpressionParser.ParseLambda<T, bool>(
            new ParsingConfig(),
            true,
            filter,
            filterParams?.ToArray() ?? Array.Empty<string>());

        return expression;
    }
}
