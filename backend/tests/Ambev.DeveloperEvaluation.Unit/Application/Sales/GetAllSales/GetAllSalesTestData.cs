using Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetAllSales;

/// <summary>
/// Provides methods for generating test data for the GetAllSales use case.
/// Centralizes the creation of read models and queries to ensure consistency
/// across application read-layer unit tests.
/// </summary>
public static class GetAllSalesTestData
{
    /// <summary>
    /// Generates a list of <see cref="GetAllSalesReadModel"/> instances.
    /// Each generated sale contains basic identification data, customer info,
    /// sale status, total items quantity and total amount.
    /// </summary>
    /// <param name="count">The number of sales to generate.</param>
    /// <returns>A read-only list of sales read models.</returns>
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

    /// <summary>
    /// Creates a <see cref="GetAllSalesQuery"/> with pagination parameters.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of records per page.</param>
    /// <returns>A configured get all sales query.</returns>
    public static GetAllSalesQuery CreateQuery(
        int page = 1,
        int pageSize = 10)
        => new(page, pageSize);
}