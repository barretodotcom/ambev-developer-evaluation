using Ambev.DeveloperEvaluation.ORM.ReadModel;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales.GetSale;

/// <summary>
/// Provides factory methods to generate test data for
/// integration tests related to retrieving multiple sales.
/// </summary>
/// <remarks>
/// This class centralizes the creation of valid <see cref="Sale"/> entities
/// to ensure consistency, readability, and reusability across tests.
/// </remarks>
public class GetSaleTests : IntegrationTestBase
{
    /// <summary>
    /// Given a valid <see cref="Sale"/> with associated <see cref="SaleItem"/>s
    /// When the sale is saved and retrieved via <see cref="SaleReadRepository.GetSaleAsync"/>
    /// Then the returned sale contains the same Id, SaleNumber, and all related items
    /// correctly loaded from the database.
    /// </summary>
    [Fact]
    public async Task GetSaleAsync_ShouldReturnSaleWithItems()
    {
        // Arrange
        var sale = GetSaleTestData.CreateValidSaleWithItems();

        DbContext.Sales.Add(sale);
        await DbContext.SaveChangesAsync();

        var repository = new SaleReadRepository(DbContext);

        // Act
        var result = await repository.GetSaleAsync(sale.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);
        result.SaleNumber.Should().Be(sale.SaleNumber);
        result.Items.Should().NotBeEmpty();
    }
}