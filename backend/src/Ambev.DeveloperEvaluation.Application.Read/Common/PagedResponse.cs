namespace Ambev.DeveloperEvaluation.Application.Read.Common;

public abstract record PagedResponse<TData> where TData : class
{
    public IReadOnlyList<TData> Data { get; init; } = new List<TData>();
    public int TotalItems { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
}