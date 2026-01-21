using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.CancelSale;

/// <summary>
/// Provides factory methods for generating test data used in
/// Cancel Sale integration tests.
/// </summary>
/// <remarks>
/// This class centralizes the creation of valid <see cref="Sale"/> aggregates
/// configured specifically for cancellation scenarios, ensuring consistent,
/// realistic, and reusable test setups across the test suite.
/// </remarks>
public class CancelSaleTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid <see cref="Sale"/> aggregate in an active state,
    /// suitable for cancellation during integration tests.
    /// </summary>
    /// <remarks>
    /// The generated sale includes:
    /// <list type="bullet">
    /// <item>Valid identifiers and metadata</item>
    /// <item>A single active sale item</item>
    /// <item>Valid quantity and unit price values</item>
    /// <item>An initial state that allows cancellation</item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// A fully initialized <see cref="Sale"/> aggregate ready to be cancelled.
    /// </returns>
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

    
}