using Ambev.DeveloperEvaluation.Application.Read.Common;
using Ambev.DeveloperEvaluation.Application.Read.Enums;
using Ambev.DeveloperEvaluation.ORM.ReadModel;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales.GetAllSales;

/// <summary>
/// Contains integration tests for retrieving sales data
/// using pagination, ordering and filtering scenarios.
/// </summary>
/// <remarks>
/// These tests validate the behavior of <see cref="SaleReadRepository"/>
/// against a real database context, ensuring correct paging,
/// ordering and empty dataset handling.
/// </remarks>
public class GetAllSalesTests : IntegrationTestBase
{
    /// <summary>
    /// When requesting the first page with ordering applied
    /// Then returns the first set of sales ordered by sale date.
    /// </summary>
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

    /// <summary>
    /// Given multiple sales
    /// When requesting the second page
    /// Then skips the first page results and returns distinct items.
    /// </summary>
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

    /// <summary>
    /// Given multiple sales
    /// When requesting the last page
    /// Then returns only the remaining items.
    /// </summary>
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

    /// <summary>
    /// Given no sales stored
    /// When requesting paginated data
    /// Then returns an empty result set with zero total items.
    /// </summary>
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