namespace Ambev.DeveloperEvaluation.WebApi.Common.Pagination;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; init; } = new List<T>();
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
}