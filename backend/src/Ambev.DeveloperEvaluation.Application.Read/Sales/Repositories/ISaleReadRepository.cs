using Ambev.DeveloperEvaluation.Application.Read.Common;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;

namespace Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;

public interface ISaleReadRepository
{
    Task<(IReadOnlyList<GetAllSalesReadModel>, int)> GetAllSalesAsync(int page, int pageSize, OrderBy? orderBy,
        CancellationToken cancellationToken);
    Task<GetSaleReadModel> GetSaleAsync(Guid saleId, CancellationToken cancellationToken);
}