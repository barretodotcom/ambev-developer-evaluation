using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.Sales.CancelSale;

/// <summary>
/// Provides factory methods to generate test data for
/// functional tests related to cancelling <see cref="Sale"/> entities via the API.
/// </summary>
/// <remarks>
/// This class centralizes the creation of valid <see cref="Sale"/> entities
/// with multiple <see cref="Domain.Entities.SaleItem"/>s to ensure consistency,
/// readability, and reusability across tests that simulate sale cancellation scenarios.
/// </remarks>
public static class CancelSaleTestData
{
    private static readonly Faker Faker = new("en");

    /// <summary>
    /// Given no initial state
    /// When creating a new sale for cancellation functional testing
    /// Then a <see cref="Sale"/> with multiple <see cref="Domain.Entities.SaleItem"/>s
    /// is returned, fully populated with realistic test data for testing cancellation operations.
    /// </summary>
    /// <returns>A valid <see cref="Sale"/> with multiple items.</returns>
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