using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Integration.Sales.CreateSale;

/// <summary>
/// Contains integration tests for the <see cref="CreateSaleCommand"/> handling flow.
/// These tests validate the full application pipeline, including
/// command handling, domain logic, persistence, interceptors,
/// and MediatR execution.
/// </summary>
public class CreateSaleTests : IntegrationTestBase
{

    public async Task<User> InsertUser()
    {
        var user = CreateSaleTestData.GenerateValidUser();
        
        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();
        return user;
    }
    /// <summary>
    /// Given a valid <see cref="CreateSaleCommand"/>
    /// When the command is handled by the application pipeline
    /// Then a new <see cref="Sale"/> is created and fully persisted in the database
    /// with all related <see cref="SaleItem"/>s correctly mapped.
    /// </summary>
    [Fact]
    public async Task Should_Create_Sale_And_Persist_In_Database()
    {
        var user = await InsertUser();
        var command = CreateSaleTestData.GenerateValidCommand(user.Id);

        var result = await Mediator.Send(command);

        result.Should().NotBeNull();

        var sale = await DbContext.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync();
        
        sale.Should().NotBeNull();

        sale!.SaleNumber.Should().Be(command.SaleNumber);
        sale.CustomerId.Should().Be(command.CustomerId);
        sale.CustomerName.Should().Be(user.Username);
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
