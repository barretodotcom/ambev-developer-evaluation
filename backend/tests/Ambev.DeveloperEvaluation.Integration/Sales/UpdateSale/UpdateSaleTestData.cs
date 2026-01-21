using Ambev.DeveloperEvaluation.Application.Enums;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.UpdateSale;

/// <summary>
/// Provides factory methods to generate test data for
/// integration tests related to updating sales.
/// </summary>
/// <remarks>
/// This class centralizes the creation of valid <see cref="Sale"/> entities and 
/// corresponding <see cref="UpdateSaleCommand"/>s to ensure consistency, readability, 
/// and reusability across update-related tests.
/// </remarks>
public class UpdateSaleTestData
{
    private static readonly Faker Faker = new("en");

    /// <summary>
    /// Given no initial state
    /// When creating a new sale with items for testing purposes
    /// Then a fully populated <see cref="Sale"/> with multiple <see cref="SaleItem"/>s is returned.
    /// </summary>
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

    /// <summary>
    /// Given an existing <see cref="Sale"/> with items
    /// When generating an <see cref="UpdateSaleCommand"/> from it
    /// Then the command contains operations to update, create, and cancel <see cref="SaleItem"/>s,
    /// reflecting realistic changes for integration testing of the update pipeline.
    /// </summary>
    public static UpdateSaleCommand GenerateValidCommandFromExistingSale(Sale sale)
    {
        var existingItem = sale.Items.First();
        
        var command = new UpdateSaleCommand()
        {
            Id = sale.Id,
            SaleNumber = "SALE-UPDATED-001",
            CustomerId = sale.CustomerId,
            CustomerName = "Updated Customer",
            Items =      new List<UpdateSaleItemCommand>()
            {
                new()
                {
                    Operation = UpdateSaleItemOperation.Update,
                    Id = existingItem.Id,
                    ProductId = existingItem.ProductId,
                    ProductName = "Updated Product",
                    Quantity = 10,
                    UnitPrice = 100
                },
                new()
                {
                    Operation = UpdateSaleItemOperation.Create,
                    Id = null,
                    ProductId = Guid.NewGuid(),
                    ProductName = "New Product",
                    Quantity = 5,
                    UnitPrice = 50
                },
                new()
                {
                    Operation = UpdateSaleItemOperation.Cancel,
                    Id = existingItem.Id,
                    ProductId = existingItem.ProductId,
                    ProductName = existingItem.ProductName,
                    Quantity = existingItem.Quantity,
                    UnitPrice = existingItem.UnitPrice.Value
                }
            },
        };

        return command;
    }
}