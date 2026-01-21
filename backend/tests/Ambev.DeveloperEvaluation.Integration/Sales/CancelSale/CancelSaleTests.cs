using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales.CancelSale;

/// <summary>
/// Contains integration tests for the sale cancellation workflow.
/// </summary>
/// <remarks>
/// These tests validate the full application pipeline, including:
/// <list type="bullet">
/// <item>Domain rules and invariants</item>
/// <item>Persistence through Entity Framework Core</item>
/// <item>Domain interceptors execution</item>
/// <item>MediatR command handling</item>
/// </list>
/// </remarks>
public sealed class CancelSaleIntegrationTests : IntegrationTestBase
{
    /// <summary>
    /// Ensures that cancelling an existing sale:
    /// - Updates the sale status to Cancelled
    /// - Cancels all related sale items
    /// - Persists the changes correctly in the database
    /// </summary>
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