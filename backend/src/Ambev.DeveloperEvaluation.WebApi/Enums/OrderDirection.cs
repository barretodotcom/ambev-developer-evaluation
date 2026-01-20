namespace Ambev.DeveloperEvaluation.WebApi.Enums;

/// <summary>
/// Specifies the direction used when ordering query results.
/// </summary>
public enum OrderDirection
{
    /// <summary>
    /// Orders results in ascending order (A → Z, 0 → 9, older → newer).
    /// </summary>
    Asc,
    /// <summary>
    /// Orders results in descending order (Z → A, 9 → 0, newer → older).
    /// </summary>
    Desc
}