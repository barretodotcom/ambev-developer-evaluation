using Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetAllSales;

public static class GetAllSalesTestData
{
    public static IReadOnlyList<GetAllSalesReadModel> CreateSales(int count = 3)
    {
        return Enumerable.Range(1, count)
            .Select(i => new GetAllSalesReadModel
            {
                Id = Guid.NewGuid(),
                SaleNumber = $"SALE-{i}",
                CustomerId = Guid.NewGuid(),
                CustomerName = $"Customer {i}",
                SaleDate = DateTime.UtcNow.AddDays(-i),
                Status = "Confirmed",
                ItemsQuantity = i * 2,
                TotalAmount = 100m * i
            })
            .ToList();
    }

    public static GetAllSalesQuery CreateQuery(
        int page = 1,
        int pageSize = 10)
        => new(page, pageSize);
}