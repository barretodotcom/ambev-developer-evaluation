using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SaleItemStatus = Ambev.DeveloperEvaluation.Domain.Enums.SaleItemStatus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.UpdateSale;

/// <summary>
/// Provides integration tests for the update sale functionality.
/// </summary>
/// <remarks>
/// This class centralizes tests that verify the correct behavior of updating 
/// <see cref="Sale"/> entities and their associated <see cref="SaleItem"/>s
/// through the application pipeline using <see cref="UpdateSaleCommand"/>.
/// It ensures that updates, creations, and cancellations of items are correctly 
/// applied and fully persisted in the database.
/// </remarks>
public class UpdateSaleIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Given an existing <see cref="Sale"/> with multiple <see cref="SaleItem"/>s
    /// When an <see cref="UpdateSaleCommand"/> is sent via the application pipeline
    /// Then the sale is updated with the new SaleNumber and CustomerName,
    /// all item operations (update, create, cancel) are correctly applied,
    /// and the changes are fully persisted in the database.
    /// </summary>
    [Fact]
    public async Task Should_Update_Sale_And_Items_Correctly()
    {
        var sale = UpdateSaleTestData.CreateValidSaleWithItems();
        DbContext.Sales.Add(sale);
        await DbContext.SaveChangesAsync();

        var command = UpdateSaleTestData.GenerateValidCommandFromExistingSale(sale);

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.Should().NotBeNull();
        result.SaleNumber.Should().Be("SALE-UPDATED-001");
        result.CustomerName.Should().Be("Updated Customer");

        // Assert
        var persistedSale = await DbContext.Sales
            .Include(x => x.Items)
            .FirstAsync(x => x.Id == sale.Id);

        persistedSale.SaleNumber.Should().Be("SALE-UPDATED-001");
        persistedSale.CustomerName.Should().Be("Updated Customer");

        persistedSale.Items.Should().HaveCount(3);

        persistedSale.Items.Should().ContainSingle(x => x.Status == SaleItemStatus.Cancelled);
        persistedSale.Items.Should().ContainSingle(x => x.ProductName == "New Product");
    }
}
