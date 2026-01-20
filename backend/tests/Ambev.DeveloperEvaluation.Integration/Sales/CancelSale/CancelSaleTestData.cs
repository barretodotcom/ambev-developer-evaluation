using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.CancelSale;

public class CancelSaleTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid Sale aggregate with item to cancel.
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

    
}