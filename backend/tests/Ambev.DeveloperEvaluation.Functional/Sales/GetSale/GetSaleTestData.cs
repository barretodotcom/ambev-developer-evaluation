using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.Sales.GetSale;

/// <summary>
/// Provides factory methods to generate test data for
/// functional tests related to retrieving <see cref="Sale"/> entities via the API.
/// </summary>
/// <remarks>
/// This class centralizes the creation of valid <see cref="Sale"/> entities and
/// their associated <see cref="Domain.Entities.SaleItem"/>s to ensure consistency,
/// readability, and reusability across get-sale functional tests.
/// </remarks>
public static class GetSaleTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Given no initial state
    /// When creating a new sale for functional testing
    /// Then a <see cref="Sale"/> with a single <see cref="Domain.Entities.SaleItem"/> is returned,
    /// fully populated with realistic test data.
    /// </summary>
    public static Sale CreateSale()
    {
        var sale = new Sale(
            Faker.Random.Guid().ToString(),
            Guid.NewGuid(),
            Faker.Name.FullName(),
            DateTime.UtcNow,
            Guid.NewGuid(),
            Faker.Company.CompanyName()
        );

        sale.AddItem(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Random.Int(1, 5),
            Money.Create(100)
        );

        return sale;
    }
}