using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.Sales.GetAllSales;

/// <summary>
/// Provides factory methods to generate test data for
/// functional tests related to retrieving multiple <see cref="Sale"/> entities via the API.
/// </summary>
/// <remarks>
/// This class centralizes the creation of multiple valid <see cref="Sale"/> entities
/// along with their associated <see cref="Domain.Entities.SaleItem"/>s for functional testing.
/// It ensures consistency, readability, and reusability across tests that require lists of sales,
/// such as pagination, filtering, or bulk retrieval scenarios.
/// </remarks>
public static class GetAllSalesTestData
{
    private static readonly Faker Faker = new();
    
    /// <summary>
    /// Given a requested number of sales
    /// When creating multiple sales for functional testing
    /// Then a list of <see cref="Sale"/> entities is returned,
    /// each containing at least one <see cref="Domain.Entities.SaleItem"/> with realistic test data.
    /// </summary>
    /// <param name="count">The number of sales to generate.</param>
    /// <returns>A list of <see cref="Sale"/> entities with populated items.</returns>
    public static List<Sale> CreateManySales(int count)
    {
        var sales = new List<Sale>();

        for (int i = 0; i < count; i++)
        {
            var sale = new Sale(
                Faker.Random.Guid().ToString(),
                Guid.NewGuid(),
                Faker.Name.FullName(),
                DateTime.UtcNow.AddMinutes(-i),
                Guid.NewGuid(),
                Faker.Company.CompanyName()
            );

            sale.AddItem(
                Guid.NewGuid(),
                Faker.Commerce.ProductName(),
                Faker.Random.Int(1, 5),
                Money.Create(100)
            );

            sales.Add(sale);
        }

        return sales;
    }
}