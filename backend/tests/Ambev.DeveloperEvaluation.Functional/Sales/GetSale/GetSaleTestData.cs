using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.Sales.GetSale;

public static class GetSaleTestData
{
    private static readonly Faker Faker = new();

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