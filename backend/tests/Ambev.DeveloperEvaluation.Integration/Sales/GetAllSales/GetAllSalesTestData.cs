using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.GetAllSales;

/// <summary>
/// Provides factory methods to generate test data for
/// integration tests related to retrieving multiple sales.
/// </summary>
/// <remarks>
/// This class centralizes the creation of valid <see cref="Sale"/> entities
/// to ensure consistency, readability, and reusability across tests.
/// </remarks>
public static class GetAllSalesTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a list of valid <see cref="Sale"/> entities with at least one item each.
    /// </summary>
    /// <param name="count">
    /// The number of sales to be generated.
    /// </param>
    /// <returns>
    /// A list of <see cref="Sale"/> entities populated with random but valid data.
    /// </returns>
    /// <remarks>
    /// Each generated sale contains:
    /// <list type="bullet">
    /// <item>A unique sale number</item>
    /// <item>Customer and branch information</item>
    /// <item>A creation date in descending order</item>
    /// <item>At least one sale item with quantity and price</item>
    /// </list>
    /// This method is intended for scenarios that require multiple persisted sales.
    /// </remarks>
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
