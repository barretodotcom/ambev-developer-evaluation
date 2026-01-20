using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSale;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleHandler"/> class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISaleRepository _saleRepository;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new CancelSaleHandler(_unitOfWork, _saleRepository);
    }

    [Fact(DisplayName = "Given valid sale When cancelling Then sale and items are cancelled")]
    public async Task Handle_ValidSale_CancelsSaleAndItems()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        sale.Status.Should().Be(SaleStatus.Active);
        sale.Items.Should().OnlyContain(i => i.Status == SaleItemStatus.Active);

        var command = new CancelSaleCommand()
        {
            Id = sale.Id,
        };

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.Items.Should().OnlyContain(i => i.Status == SaleItemStatus.Cancelled);

        _saleRepository.Received(1).Update(sale);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non existing sale When cancelling Then throws exception")]
    public async Task Handle_SaleNotFound_ThrowsException()
    {
        // Arrange
        var command = new CancelSaleCommand()
        {
            Id = Guid.NewGuid(),
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("The sale does not exist.");
    }

    [Fact(DisplayName = "Given already cancelled sale When cancelling Then throws domain exception")]
    public async Task Handle_AlreadyCancelledSale_ThrowsException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();

        var command = new CancelSaleCommand()
        {
            Id = sale.Id,
        };

        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>())
            .Returns(sale);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}