using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for the Sale aggregate using the Bogus library.
/// This class centralizes all Sale test data generation to ensure consistency
/// across unit tests, including both valid and invalid scenarios.
/// </summary>
public static class SaleTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid Sale aggregate with at least one item.
    /// The generated sale will have:
    /// - A valid identifier and customer information
    /// - An active status
    /// - At least one SaleItem with:
    ///   - Valid product identifier and name
    ///   - Quantity within the allowed range
    ///   - Unit price greater than zero
    /// </summary>
    public static Sale GenerateValidSale()
    {
        var sale = new Sale(Faker.Random.String(), Faker.Random.Guid(), Faker.Random.String(), DateTime.UtcNow, Faker.Random.Guid(),Faker.Random.String());

        sale.AddItem(
            productId: Faker.Random.Guid(),
            productName: Faker.Commerce.ProductName(),
            quantity: Faker.Random.Int(1, 10),
            unitPrice: Money.Create(Faker.Random.Decimal(10, 100))
        );

        return sale;
    }

    /// <summary>
    /// Generates a Sale aggregate without items.
    /// This scenario is useful for testing behaviors that depend on
    /// an empty sale, such as initial creation or validation rules
    /// that require at least one item.
    /// </summary>
    public static Sale GenerateSaleWithoutItems()
    {
        return new Sale(Faker.Random.String(), Faker.Random.Guid(), Faker.Random.String(), DateTime.UtcNow, Faker.Random.Guid(),Faker.Random.String());
    }
}
