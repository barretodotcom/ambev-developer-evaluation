namespace Ambev.DeveloperEvaluation.ORM.Extensions;

/// <summary>
/// Queryable Extensions to LINQ database queries.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Skips the specified number of elements if the condition is true.
    /// </summary>
    public static IQueryable<T> SkipIf<T>(this IQueryable<T> query, bool condition, int count)
    {
        return condition ? query.Skip(count) : query;
    }

    /// <summary>
    /// Takes the specified number of elements if the condition is true.
    /// </summary>
    public static IQueryable<T> TakeIf<T>(this IQueryable<T> query, bool condition, int count)
    {
        return condition ? query.Take(count) : query;
    }
}