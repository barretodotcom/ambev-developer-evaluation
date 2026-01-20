using Ambev.DeveloperEvaluation.Application.Enums;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Functional.Sales.UpdateSale;

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
    
    public static UpdateSaleCommand GenerateValidCommandFromExistingSale(Sale sale)
    {
        var existingItem = sale.Items.First();

        var command = new UpdateSaleCommand()
        {
            Id = sale.Id,
            SaleNumber = $"SALE-{Faker.Random.AlphaNumeric(6).ToUpper()}",
            CustomerId = sale.CustomerId,
            CustomerName = Faker.Person.FullName,
            Items = new List<UpdateSaleItemCommand>()
            {
                new UpdateSaleItemCommand
                {
                    Operation = UpdateSaleItemOperation.Update,
                    Id = existingItem.Id,
                    ProductId = existingItem.ProductId,
                    ProductName = Faker.Commerce.ProductName(),
                    Quantity = Faker.Random.Int(1, 20),
                    UnitPrice = Faker.Finance.Amount(10, 500)
                },
                new UpdateSaleItemCommand
                {
                    Operation = UpdateSaleItemOperation.Create,
                    Id = null,
                    ProductId = Guid.NewGuid(),
                    ProductName = Faker.Commerce.ProductName(),
                    Quantity = Faker.Random.Int(1, 10),
                    UnitPrice = Faker.Finance.Amount(5, 200)
                },
                new UpdateSaleItemCommand
                {
                    Operation = UpdateSaleItemOperation.Cancel,
                    Id = existingItem.Id,
                    ProductId = existingItem.ProductId,
                    ProductName = existingItem.ProductName,
                    Quantity = existingItem.Quantity,
                    UnitPrice = existingItem.UnitPrice.Value
                }
            }
        };

        return command;
    }
}