using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();

        _handler = new CreateSaleHandler(
            _saleRepository,
            _unitOfWork,
            _mapper
        );
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleTestData.GenerateValidCommand();

        var sale = new Sale(
            command.SaleNumber,
            command.CustomerId,
            command.CustomerName,
            command.SaleDate,
            command.BranchId,
            command.BranchName
        );

        var result = new CreateSaleResult()
        {
            Id = sale.Id
        };

        _saleRepository
            .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        // When
        var response = await _handler.Handle(command, CancellationToken.None);

        // Then
        response.Should().NotBeNull();
        response.Id.Should().Be(sale.Id);

        await _saleRepository.Received(1)
            .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());

        await _unitOfWork.Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid sale When handling Then commits unit of work")]
    public async Task Handle_ValidRequest_CommitsUnitOfWork()
    {
        // Given
        var command = CreateSaleTestData.GenerateValidCommand();

        _saleRepository
            .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Sale>());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _unitOfWork.Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid sale When handling Then maps sale to result")]
    public async Task Handle_ValidRequest_MapsSaleToResult()
    {
        // Given
        var command = CreateSaleTestData.GenerateValidCommand();

        _saleRepository
            .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Sale>());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1)
            .Map<CreateSaleResult>(Arg.Any<Sale>());
    }

    [Fact(DisplayName = "Given sale with items When handling Then creates sale with items")]
    public async Task Handle_ValidRequest_CreatesSaleWithItems()
    {
        // Given
        var command = CreateSaleTestData.GenerateValidCommand();

        Sale? capturedSale = null;

        _saleRepository
            .CreateAsync(Arg.Do<Sale>(s => capturedSale = s), Arg.Any<CancellationToken>())
            .Returns(ci => ci.Arg<Sale>());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        capturedSale!.Items.Should().HaveCount(command.Items.Count);
    }
}