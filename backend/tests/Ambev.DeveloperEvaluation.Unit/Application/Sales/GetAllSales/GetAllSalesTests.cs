using Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetAllSales;

public sealed class GetAllSalesHandlerTests
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly GetAllSalesHandler _handler;

    public GetAllSalesHandlerTests()
    {
        _saleReadRepository = Substitute.For<ISaleReadRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GetAllSalesProfile>();
        });

        var mapper = mapperConfig.CreateMapper();

        _handler = new GetAllSalesHandler(
            _saleReadRepository,
            mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedResult_WhenSalesExist()
    {
        // Arrange
        var query = GetAllSalesTestData.CreateQuery(page: 1, pageSize: 10);
        var sales = GetAllSalesTestData.CreateSales(3);
        var totalItems = 3;

        _saleReadRepository
            .GetAllSalesAsync(
                query.Page,
                query.PageSize,
                null,
                Arg.Any<CancellationToken>())
            .Returns((sales, totalItems));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(query.Page, result.CurrentPage);
        Assert.Equal(query.PageSize, result.PageSize);
        Assert.Equal(totalItems, result.TotalItems);
        Assert.Equal(3, result.Data.Count);

        await _saleReadRepository.Received(1)
            .GetAllSalesAsync(
                query.Page,
                query.PageSize,
                null,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoSalesExist()
    {
        // Arrange
        var query = GetAllSalesTestData.CreateQuery(page: 1, pageSize: 10);
        var emptySales = new List<GetAllSalesReadModel>();
        var totalItems = 0;

        _saleReadRepository
            .GetAllSalesAsync(
                query.Page,
                query.PageSize,
                null,
                Arg.Any<CancellationToken>())
            .Returns((emptySales, totalItems));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(query.Page, result.CurrentPage);
        Assert.Equal(query.PageSize, result.PageSize);
        Assert.Equal(0, result.TotalItems);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);

        await _saleReadRepository.Received(1)
            .GetAllSalesAsync(
                query.Page,
                query.PageSize,
                null,
                Arg.Any<CancellationToken>());
    }
}