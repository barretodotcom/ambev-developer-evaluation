using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();

        _handler = new UpdateSaleHandler(_saleRepository, _unitOfWork, _mapper);
    }

    [Fact(DisplayName = "Given valid command When updating sale Then commits and returns result")]
    public async Task Handle_ValidCommand_CommitsAndReturnsResult()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var command = UpdateSaleHandlerTestData.GenerateValidCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        var result = CreateUpdateSaleResult();

        _mapper.Map<UpdateSaleResult>(sale).Returns(result);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();

        await _unitOfWork.Received(1)
            .CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non existing sale When handling Then throws exception")]
    public async Task Handle_SaleNotFound_ThrowsException()
    {
        // Arrange
        var command = UpdateSaleHandlerTestData.GenerateValidCommand(Guid.NewGuid());

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("The sale does not exist.");
    }

    [Fact(DisplayName = "Given cancelled sale When updating Then throws exception")]
    public async Task Handle_CancelledSale_ThrowsException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();

        var command = UpdateSaleHandlerTestData.GenerateValidCommand(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Sale is cancelled and cannot be updated.");
    }

    [Fact(DisplayName = "Given item create operation When handling Then adds item")]
    public async Task Handle_CreateItemOperation_AddsItem()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithoutItems();
        var command = UpdateSaleHandlerTestData.GenerateCommandWithCreateItem(sale.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<UpdateSaleResult>(sale)
            .Returns(CreateUpdateSaleResult());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Items.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Given item update operation When handling Then updates item")]
    public async Task Handle_UpdateItemOperation_UpdatesItem()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var existingItem = sale.Items.First();

        var command = UpdateSaleHandlerTestData.GenerateCommandWithUpdateItem(
            sale.Id, existingItem.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<UpdateSaleResult>(sale)
            .Returns(CreateUpdateSaleResult());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Items.First().Quantity
            .Should().Be(command.Items.First().Quantity);
    }

    [Fact(DisplayName = "Given item cancel operation When handling Then cancels item")]
    public async Task Handle_CancelItemOperation_CancelsItem()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var item = sale.Items.First();

        var command = UpdateSaleHandlerTestData.GenerateCommandWithCancelItem(
            sale.Id, item.Id);

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<UpdateSaleResult>(sale)
            .Returns(CreateUpdateSaleResult());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        item.Status.Should().Be(SaleItemStatus.Cancelled);
    }

    private static UpdateSaleResult CreateUpdateSaleResult()
    {
        return (UpdateSaleResult)Activator
            .CreateInstance(typeof(UpdateSaleResult), nonPublic: true)!;
    }
}
