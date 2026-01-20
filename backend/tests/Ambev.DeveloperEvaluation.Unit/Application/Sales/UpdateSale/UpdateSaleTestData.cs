using Ambev.DeveloperEvaluation.Application.Enums;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale;

public static class UpdateSaleHandlerTestData
{
    private static readonly Faker Faker = new();

    public static UpdateSaleCommand GenerateValidCommand(Guid saleId)
    {
        return new UpdateSaleCommand()
        {
            Id = saleId,
            CustomerId = Faker.Random.Guid(),
            SaleNumber = Faker.Random.String(),
            CustomerName = Faker.Person.FullName,
            Items = [GenerateCreateItem()],
        };
    }

    public static UpdateSaleCommand GenerateCommandWithCreateItem(Guid saleId)
    {
        return new UpdateSaleCommand()
        {
            Id = saleId,
            CustomerId = Faker.Random.Guid(),
            SaleNumber = Faker.Random.String(),
            CustomerName = Faker.Person.FullName,
            Items = [GenerateCreateItem()],
        };
    }

    public static UpdateSaleCommand GenerateCommandWithUpdateItem(Guid saleId, Guid itemId)
    {
        return new UpdateSaleCommand()
        {
            Id = saleId,
            SaleNumber = Faker.Random.String(),
            CustomerId = Faker.Random.Guid(),
            CustomerName = Faker.Person.FullName,
            Items = [GenerateUpdateItem(itemId)],
        };
    }

    public static UpdateSaleCommand GenerateCommandWithCancelItem(Guid saleId, Guid itemId)
    {
        return new UpdateSaleCommand()
        {
            Id = saleId,
            SaleNumber = Faker.Random.String(),
            CustomerId = Faker.Random.Guid(),
            CustomerName = Faker.Person.FullName,
            Items = [GenerateCancelItem(itemId)],
        };
    }

    private static UpdateSaleItemCommand GenerateCreateItem()
    {
        return new UpdateSaleItemCommand()
        {
            Operation = UpdateSaleItemOperation.Create,
            ProductId = Faker.Random.Guid(),
            ProductName = Faker.Commerce.ProductName(),
            Quantity = Faker.Random.Int(1, 5),
            UnitPrice = Faker.Random.Decimal(10, 100)
        };
    }

    private static UpdateSaleItemCommand GenerateUpdateItem(Guid id)
    {
        return new UpdateSaleItemCommand()
        {
            Id = id,
            Operation = UpdateSaleItemOperation.Update,
            ProductId = Faker.Random.Guid(),
            ProductName = Faker.Commerce.ProductName(),
            Quantity = Faker.Random.Int(1, 5),
            UnitPrice = Faker.Random.Decimal(10, 100)
        };
    }

    private static UpdateSaleItemCommand GenerateCancelItem(Guid id)
    {
        return new UpdateSaleItemCommand()
        {
            Id = id,
            Operation = UpdateSaleItemOperation.Cancel,
        };
    }
}