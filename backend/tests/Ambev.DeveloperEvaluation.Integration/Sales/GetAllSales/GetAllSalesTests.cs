using Ambev.DeveloperEvaluation.Application.Read.Common;
using Ambev.DeveloperEvaluation.Application.Read.Enums;
using Ambev.DeveloperEvaluation.ORM.ReadModel;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales.GetAllSales;

public class GetAllSalesTests : IntegrationTestBase
{
    [Fact]
    public async Task GetAllSalesAsync_Page1_ShouldReturnFirstPage()
    {
        // Arrange
        var sales = GetAllSalesTestData.CreateManySales(5);
        DbContext.Sales.AddRange(sales);
        await DbContext.SaveChangesAsync();

        var repository = new SaleReadRepository(DbContext);

        var orderBy = new OrderBy()
        {
            Direction = OrderDirection.Asc,
            Field = "saleDate"
        };
        
        // Act
        var (data, totalItems) = await repository.GetAllSalesAsync(
            1,
            2,
            orderBy,
            CancellationToken.None);

        // Assert
        totalItems.Should().Be(5);
        data.Should().HaveCount(2);

        data.Should().BeInAscendingOrder(x => x.SaleDate);
    }

    [Fact]
    public async Task GetAllSalesAsync_Page2_ShouldSkipFirstPage()
    {
        // Arrange
        var sales = GetAllSalesTestData.CreateManySales(5);
        DbContext.Sales.AddRange(sales);
        await DbContext.SaveChangesAsync();

        var repository = new SaleReadRepository(DbContext);

        // Act
        var (page1, _) = await repository.GetAllSalesAsync(1, 2, null, CancellationToken.None);
        var (page2, _) = await repository.GetAllSalesAsync(2, 2, null, CancellationToken.None);

        // Assert
        page2.Should().HaveCount(2);
        page2.Select(x => x.Id)
            .Should()
            .NotIntersectWith(page1.Select(x => x.Id));
    }

    [Fact]
    public async Task GetAllSalesAsync_LastPage_ShouldReturnRemainingItems()
    {
        // Arrange
        var sales = GetAllSalesTestData.CreateManySales(5);
        DbContext.Sales.AddRange(sales);
        await DbContext.SaveChangesAsync();

        var repository = new SaleReadRepository(DbContext);

        // Act
        var (data, totalItems) = await repository.GetAllSalesAsync(
            3,
            2,
            null,
            CancellationToken.None);

        // Assert
        totalItems.Should().Be(5);
        data.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAllSalesAsync_WhenNoSales_ShouldReturnEmptyList()
    {
        // Arrange
        var repository = new SaleReadRepository(DbContext);

        // Act
        var (data, totalItems) = await repository.GetAllSalesAsync(
            1,
            10,
            null,
            CancellationToken.None);

        // Assert
        totalItems.Should().Be(0);
        data.Should().BeEmpty();
    }
}