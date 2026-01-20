using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales.CancelSale;

public sealed class CancelSaleIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Cancel_Sale_And_Items_And_Persist_In_Database()
    {
        // Arrange
        var sale = CancelSaleTestData.GenerateValidSale();

        DbContext.Sales.Add(sale);
        await DbContext.SaveChangesAsync();

        var command = new CancelSaleCommand()
        {
            Id = sale.Id,
        };

        // Act
        await Mediator.Send(command);

        // Assert
        var cancelledSale = await DbContext.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == sale.Id);

        cancelledSale.Should().NotBeNull();
        cancelledSale!.Status.Should().Be(SaleStatus.Cancelled);

        cancelledSale.Items.Should().NotBeEmpty();
        cancelledSale.Items.Should()
            .OnlyContain(i => i.Status == SaleItemStatus.Cancelled);
    }
}