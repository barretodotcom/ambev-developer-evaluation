using Ambev.DeveloperEvaluation.Application.Read.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public sealed class GetSaleHandlerTests
{
    private readonly ISaleReadRepository _saleReadRepository;
    private readonly GetSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandlerTests"/> class.
    /// Configures the read repository mock and AutoMapper profile.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleReadRepository = Substitute.For<ISaleReadRepository>();

        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<GetSaleProfile>(); });

        var mapper = mapperConfig.CreateMapper();

        _handler = new GetSaleHandler(
            _saleReadRepository,
            mapper);
    }

    /// <summary>
    /// Tests that when an existing sale is queried,
    /// the handler returns the mapped sale result.
    /// </summary>
    [Fact(DisplayName = "Given existing sale When handling Then returns mapped sale")]
    public async Task Handle_ShouldReturnSale_WhenSaleExists()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var query = new GetSaleQuery()
        {
            Id = saleId,
        };

        var readModel = GetSaleTestData.CreateReadModel(saleId);

        _saleReadRepository
            .GetSaleAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(readModel);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(readModel.Id);
        result.SaleNumber.Should().Be(readModel.SaleNumber);
        result.CustomerId.Should().Be(readModel.CustomerId);
        result.CustomerName.Should().Be(readModel.CustomerName);

        await _saleReadRepository.Received(1)
            .GetSaleAsync(saleId, Arg.Any<CancellationToken>());
    }
}