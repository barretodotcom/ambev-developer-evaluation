using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Common.Pagination;

public abstract record PaginationQueryParams
{
    [FromQuery(Name = "_page")] public int Page { get; init; } = 1;
    [FromQuery(Name = "_size")] public int PageSize { get; init; } = 10;
    [FromQuery(Name = "_order")] public OrderBy? Order { get; init; }
}