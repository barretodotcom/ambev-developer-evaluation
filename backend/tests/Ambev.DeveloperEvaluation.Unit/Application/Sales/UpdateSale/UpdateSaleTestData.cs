using Ambev.DeveloperEvaluation.Application.Enums;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale;

/// <summary>
/// Provides methods for generating test data for the Update Sale application handler.
/// Centralizes the creation of valid update commands with different item scenarios
/// to ensure consistency across unit tests.
/// </summary>
public static class UpdateSaleHandlerTestData
{
    private static readonly Faker Faker = new();

    /// <summary>
    /// Generates a valid <see cref="UpdateSaleCommand"/> containing a new item.
    /// </summary>
    /// <param name="saleId">The identifier of the sale to be updated.</param>
    /// <returns>A valid update sale command.</returns>
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

    /// <summary>
    /// Generates an <see cref="UpdateSaleCommand"/> configured to create a new sale item.
    /// </summary>
    /// <param name="saleId">The identifier of the sale to be updated.</param>
    /// <returns>An update sale command with a create-item instruction.</returns>
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

    /// <summary>
    /// Generates an <see cref="UpdateSaleCommand"/> configured to update an existing sale item.
    /// </summary>
    /// <param name="saleId">The identifier of the sale to be updated.</param>
    /// <param name="itemId">The identifier of the item to be updated.</param>
    /// <returns>An update sale command with an update-item instruction.</returns>
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
    
    /// <summary>
    /// Generates an <see cref="UpdateSaleCommand"/> configured to cancel an existing sale item.
    /// </summary>
    /// <param name="saleId">The identifier of the sale to be updated.</param>
    /// <param name="itemId">The identifier of the item to be cancelled.</param>
    /// <returns>An update sale command with a cancel-item instruction.</returns>
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
    
    /// <summary>
    /// Generates an <see cref="UpdateSaleItemCommand"/> configured to create a new sale item.
    /// </summary>
    /// <returns>A create-item update command.</returns>
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

    /// <summary>
    /// Generates an <see cref="UpdateSaleItemCommand"/> configured to update an existing sale item.
    /// </summary>
    /// <param name="id">The identifier of the item to be updated.</param>
    /// <returns>An update-item command.</returns>
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

    /// <summary>
    /// Generates an <see cref="UpdateSaleItemCommand"/> configured to cancel an existing sale item.
    /// </summary>
    /// <param name="id">The identifier of the item to be cancelled.</param>
    /// <returns>A cancel-item update command.</returns>
    private static UpdateSaleItemCommand GenerateCancelItem(Guid id)
    {
        return new UpdateSaleItemCommand()
        {
            Id = id,
            Operation = UpdateSaleItemOperation.Cancel,
        };
    }
}