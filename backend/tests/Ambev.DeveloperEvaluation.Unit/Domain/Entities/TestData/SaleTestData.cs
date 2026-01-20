using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Sale aggregate using Bogus.
/// Centralizes valid and invalid data generation for consistent unit tests.
/// </summary>
public static class SaleTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid Sale aggregate with items.
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
    /// Generates a Sale without items (invalid scenario).
    /// </summary>
    public static Sale GenerateSaleWithoutItems()
    {
        return new Sale(Faker.Random.String(), Faker.Random.Guid(), Faker.Random.String(), DateTime.UtcNow, Faker.Random.Guid(),Faker.Random.String());
    }

    /// <summary>
    /// Generates an invalid quantity (zero or negative).
    /// </summary>
    public static int GenerateInvalidQuantity()
    {
        return Faker.Random.Int(-10, 0);
    }

    /// <summary>
    /// Generates a valid quantity.
    /// </summary>
    public static int GenerateValidQuantity()
    {
        return Faker.Random.Int(1, 20);
    }

    /// <summary>
    /// Generates an invalid unit price (zero or negative).
    /// </summary>
    public static Money GenerateInvalidUnitPrice()
    {
        return Money.Create(Faker.Random.Decimal(-100, 0));
    }

    /// <summary>
    /// Generates a valid unit price.
    /// </summary>
    public static Money GenerateValidUnitPrice()
    {
        return Money.Create(Faker.Random.Decimal(1, 500));
    }

    /// <summary>
    /// Generates a valid product name.
    /// </summary>
    public static string GenerateValidProductName()
    {
        return Faker.Commerce.ProductName();
    }
}
