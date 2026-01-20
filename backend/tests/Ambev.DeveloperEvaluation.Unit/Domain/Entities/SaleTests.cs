using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void AddItem_ShouldIncreaseQuantity_WhenProductAlreadyExists()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithoutItems();

        var productId = Guid.NewGuid();
            
        // Act
        sale.AddItem(
            productId:productId ,
            productName: "Product A",
            quantity: 2,
            unitPrice: Money.Create(10)
        );
        sale.AddItem(
            productId: productId,
            productName: "Product A",
            quantity: 2,
            unitPrice: Money.Create(10)
        );

        // Assert
        sale.Items.Count.Should().Be(1);
        sale.Items.First().Quantity.Should().Be(4);
    }

    
    [Fact]
    public void AddItem_ShouldIncreaseTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithoutItems();

        // Act
        sale.AddItem(
            productId: Guid.NewGuid(),
            productName: "Product A",
            quantity: 2,
            unitPrice: Money.Create(10)
        );
        sale.AddItem(
            productId: Guid.NewGuid(),
            productName: "Product B",
            quantity: 2,
            unitPrice: Money.Create(10)
        );

        // Assert
        sale.TotalAmount.Value.Should().Be(40);
    }
    
    [Fact]
    public void Constructor_ShouldThrowException_WhenSaleDateIsInFuture()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        var act = () => new Sale(
            "SALE-001",
            Guid.NewGuid(),
            "Customer",
            futureDate,
            Guid.NewGuid(),
            "Branch");

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Sale date cannot be in the future");
    }

    [Fact]
    public void Constructor_ShouldCreateSaleAndRaiseDomainEvent()
    {
        // Arrange & Act
        var sale = SaleTestData.GenerateSaleWithoutItems();

        // Assert
        sale.Status.Should().Be(SaleStatus.Active);
        sale.DomainEvents.Should()
            .ContainSingle(e => e.GetType().Name == "SaleCreatedDomainEvent");
    }

    [Fact]
    public void AddItem_ShouldAddItem_WhenProductIsNew()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithoutItems();

        // Act
        sale.AddItem(
            Guid.NewGuid(),
            "Product A",
            2,
            Money.Create(10));

        // Assert
        sale.Items.Should().HaveCount(1);
        sale.TotalAmount.Value.Should().Be(20);
    }

    [Fact]
    public void AddItem_ShouldIncreaseQuantity_WhenSameProductIsAdded()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithoutItems();
        var productId = Guid.NewGuid();

        sale.AddItem(productId, "Product A", 5, Money.Create(10));

        // Act
        sale.AddItem(productId, "Product A", 3, Money.Create(10));

        // Assert
        var item = sale.Items.Single();
        item.Quantity.Should().Be(8);
    }

    [Fact]
    public void AddItem_ShouldThrowException_WhenQuantityExceedsLimit()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithoutItems();
        var productId = Guid.NewGuid();

        // Act
        var act = () => sale.AddItem(
            productId,
            "Product A",
            21,
            Money.Create(10));

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("Cannot sell more than 20 identical items.");
    }

    [Theory]
    [InlineData(4, 0.10)]
    [InlineData(10, 0.20)]
    public void AddItem_ShouldApplyCorrectDiscount(int quantity, decimal expectedDiscount)
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithoutItems();

        // Act
        sale.AddItem(
            Guid.NewGuid(),
            "Product A",
            quantity,
            Money.Create(100));

        // Assert
        sale.Items.Single().DiscountPercentage.Value.Should().Be(expectedDiscount);
    }

    [Fact]
    public void UpdateItem_ShouldUpdateItem_WhenValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var item = sale.Items.First();

        // Act
        sale.UpdateItem(
            item.Id,
            item.ProductId,
            "Updated Product",
            5,
            Money.Create(50));

        // Assert
        item.Quantity.Should().Be(5);
        item.ProductName.Should().Be("Updated Product");
    }

    [Fact]
    public void UpdateItem_ShouldThrowException_WhenItemDoesNotExist()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var act = () => sale.UpdateItem(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Product",
            1,
            Money.Create(10));

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("*not found*");
    }

    [Fact]
    public void UpdateItem_ShouldCancelDuplicatedProduct()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithoutItems();
        var productId = Guid.NewGuid();

        sale.AddItem(productId, "Product A", 2, Money.Create(10));
        sale.AddItem(Guid.NewGuid(), "Product B", 1, Money.Create(10));

        var firstItem = sale.Items.First();
        var secondItem = sale.Items.Last();

        // Act
        sale.UpdateItem(
            firstItem.Id,
            secondItem.ProductId,
            "Merged Product",
            3,
            Money.Create(10));

        // Assert
        secondItem.Status.Should().Be(SaleItemStatus.Cancelled);
    }

    [Fact]
    public void CancelItem_ShouldCancelItem_WhenExists()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var item = sale.Items.First();

        // Act
        sale.CancelItem(item.Id);

        // Assert
        item.Status.Should().Be(SaleItemStatus.Cancelled);
    }

    [Fact]
    public void Update_ShouldRaiseSaleUpdatedDomainEvent()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.Update("NEW-SALE", Guid.NewGuid(), "New Customer");

        // Assert
        sale.DomainEvents.Should()
            .ContainSingle(e => e.GetType().Name == "SaleUpdatedDomainEvent");
    }

    [Fact]
    public void Cancel_ShouldCancelSaleAndRaiseEvent()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.Cancel();

        // Assert
        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.DomainEvents.Should()
            .ContainSingle(e => e.GetType().Name == "SaleCancelledDomainEvent");
    }

    [Fact]
    public void Cancel_ShouldThrowException_WhenAlreadyCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel();

        // Act
        var act = () => sale.Cancel();

        // Assert
        act.Should()
           .Throw<DomainException>()
           .WithMessage("The sale is already cancelled.");
    }
}