using Ambev.DeveloperEvaluation.WebApi.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Common.Pagination;

/// <summary>
/// Represents an ordering definition for a query result.
/// Defines which field should be used for ordering and the direction of the order.
/// </summary>
public record OrderBy
{
    /// <summary>
    /// The name of the field to be used for sorting.
    /// Must be one of the allowed/whitelisted fields for the query.
    /// </summary>
    public string Field { get; init; } = string.Empty;

    /// <summary>
    /// The direction in which the results should be ordered (ascending or descending).
    /// </summary>
    public OrderDirection Direction { get; init; }
}