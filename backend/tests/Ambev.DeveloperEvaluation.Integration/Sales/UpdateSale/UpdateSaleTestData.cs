using Ambev.DeveloperEvaluation.Application.Enums;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.UpdateSale;

public class UpdateSaleTestData
{
    private static readonly Faker Faker = new("en");

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