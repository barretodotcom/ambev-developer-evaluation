using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Integration.Sales.CreateSale;

public class CreateSaleIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task Should_Create_Sale_And_Persist_In_Database()
    {
        var command = CreateSaleTestData.GenerateValidCommand();

        var result = await Mediator.Send(command);

        result.Should().NotBeNull();

        var sale = await DbContext.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync();
        
        sale.Should().NotBeNull();

        sale!.SaleNumber.Should().Be(command.SaleNumber);
        sale.CustomerId.Should().Be(command.CustomerId);
        sale.CustomerName.Should().Be(command.CustomerName);
        sale.BranchId.Should().Be(command.BranchId);
        sale.BranchName.Should().Be(command.BranchName);
        sale.SaleDate.Should().BeCloseTo(command.SaleDate, TimeSpan.FromSeconds(1));

        sale.Items.Should().NotBeEmpty();
        sale.Items.Count.Should().Be(command.Items.Count);

        var commandItem = command.Items.First();
        var saleItem = sale.Items.First();

        saleItem.ProductId.Should().Be(commandItem.ProductId);
        saleItem.ProductName.Should().Be(commandItem.ProductName);
        saleItem.Quantity.Should().Be(commandItem.Quantity);
        saleItem.UnitPrice.Value.Should().Be(commandItem.UnitPrice);
    }
}
