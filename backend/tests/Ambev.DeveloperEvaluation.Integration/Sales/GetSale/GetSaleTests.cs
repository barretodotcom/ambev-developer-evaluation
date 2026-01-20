using Ambev.DeveloperEvaluation.ORM.ReadModel;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales.GetSale;

public class GetSaleTests : IntegrationTestBase
{
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