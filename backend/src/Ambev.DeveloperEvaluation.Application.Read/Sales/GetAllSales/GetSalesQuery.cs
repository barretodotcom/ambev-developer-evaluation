using Ambev.DeveloperEvaluation.Application.Read.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;

public record GetAllSalesQuery(
    int Page = 1,
    int PageSize = 10,
    OrderBy? OrderBy = null) : IRequest<GetAllSalesPagedResult>;