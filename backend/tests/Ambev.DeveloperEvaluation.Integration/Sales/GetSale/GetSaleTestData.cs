using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.GetSale;

/// <summary>
/// Provides methods for generating test data for GetSale integration tests.
/// This class centralizes the creation of fully populated <see cref="Sale"/> aggregates,
/// ensuring consistency and realistic persisted states across integration scenarios.
/// </summary>
public class GetSaleTestData
{
    private static readonly Faker Faker = new("en");

    /// <summary>
    /// Generates a valid Sale aggregate with multiple items.
    /// The generated sale contains:
    /// - A valid sale number
    /// - Customer and branch information
    /// - A recent sale date
    /// - Multiple active sale items with valid quantities and prices
    ///
    /// This data is intended to be persisted in the database and used
    /// to validate read operations, mappings, and query handlers.
    /// </summary>
    /// <returns>A valid <see cref="Sale"/> aggregate with items.</returns>
    public static Sale CreateValidSaleWithItems()
    {
        var sale = new Sale(
            Faker.Commerce.Ean13(),
            Faker.Random.Guid(),
            Faker.Name.FullName(),
            Faker.Date.Recent(10),
            Faker.Random.Guid(),
            Faker.Commerce.Department()
        );

        sale.AddItem(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Random.Int(1, 10),
            Money.Create(Faker.Random.Decimal(10, 500))
        );

        sale.AddItem(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Random.Int(1, 10),
            Money.Create(Faker.Random.Decimal(10, 500))
        );

        return sale;
    }
}