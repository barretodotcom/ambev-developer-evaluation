using Ambev.DeveloperEvaluation.Application.Enums;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SaleItemStatus = Ambev.DeveloperEvaluation.Domain.Enums.SaleItemStatus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.UpdateSale;

public class UpdateSaleIntegrationTests : IntegrationTestBase
{
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
