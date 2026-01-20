using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// This aggregate root encapsulates all business rules related to a sale:
/// customer information, sale items, pricing, discounts, totals and current state (active or cancelled).
/// This entity follows domain-driven design principles and includes business rules validation.
/// External entities (such as Customer, Product, and Branch) are referenced
/// using their identifiers, following the External Identity pattern to
/// maintain bounded context isolation.
/// </summary>
public sealed class Sale : AggregateRoot
{
    /// <summary>
    /// Gets the sale number.
    /// Must not be null or empty.
    /// </summary>
    public string SaleNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the customer id.
    /// Must not be empty.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Gets the customer name.
    /// Must not be null or empty.
    /// </summary>
    public string CustomerName { get; private set; }

    /// <summary>
    /// Gets the date when the sale was made.
    /// The value must be specified and cannot be in the future.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the branch id
    /// Must not be empty.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the branch name.
    /// Must not be null or empty.
    /// </summary>
    public string BranchName { get; private set; }

    /// <summary>
    /// Gets the date when the sale entity was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time of the last update to the sale's information.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the sale status.
    /// Default "Active".
    /// Could not be initialized as "Cancelled".
    /// </summary>
    public SaleStatus Status { get; private set; }

    /// <summary>
    /// Gets the total items' quantity.
    /// </summary>
    public int ItemsQuantity => _items.Sum(l => l.Quantity);

    /// <summary>
    /// Gets the total amount.
    /// </summary>
    public Money TotalAmount => Money.Create(_items.Sum(l => l.TotalAmount.Value));

    private readonly List<SaleItem> _items = new();

    /// <summary>
    /// Gets the sales items.
    /// </summary>
    public IReadOnlyList<SaleItem> Items => _items.AsReadOnly();

    private Sale()
    {
    }

    public Sale(string saleNumber, Guid customerId, string customerName, DateTime saleDate, Guid branchId, string branchName)
    {
        if (saleDate > DateTime.UtcNow)
            throw new DomainException("Sale date cannot be in the future");

        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CreatedAt = DateTime.UtcNow;

        CustomerId = customerId;
        CustomerName = customerName;

        BranchId = branchId;
        BranchName = branchName;

        Status = SaleStatus.Active;

        AddDomainEvent(new SaleCreatedDomainEvent(Id, CustomerId, SaleDate));
    }

    public void AddItem(Guid productId, string productName, int quantity, Money unitPrice)
    {
        var existingItem = _items.FirstOrDefault(l => l.ProductId == productId && l.Status != SaleItemStatus.Cancelled);

        var totalQuantity = existingItem is null ? quantity : existingItem.Quantity + quantity;

        if (totalQuantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items.");

        var discountPercentage = CalculateDiscount(totalQuantity);

        if (existingItem is null)
        {
            var item = new SaleItem(
                productId,
                productName,
                totalQuantity,
                unitPrice,
                discountPercentage);

            _items.Add(item);
        }
        else
        {
            existingItem.UpdateQuantityAndDiscount(totalQuantity, discountPercentage);
        }
    }

    public void UpdateItem(Guid id, Guid productId, string productName, int quantity, Money unitPrice)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id == id);

        if (existingItem is null)
            throw new DomainException($"Item {id} not found.");

        if (existingItem.Status is SaleItemStatus.Cancelled)
            throw new DomainException("Item is cancelled and cannot be updated.");
        
        var existingItemProduct = _items.FirstOrDefault(i => i.ProductId == productId && i.Id != id);

        if (quantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items.");

        var discountPercentage = CalculateDiscount(quantity);

        existingItem.Update(quantity, discountPercentage, productId, productName, unitPrice);
        
        if (existingItemProduct is not null)
        {
            existingItemProduct.Cancel();
        }
    }

    public void CancelItem(Guid id)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id == id);
        if (existingItem is null)
            throw new DomainException($"Item {id} not found.");

        if (existingItem.Status == SaleItemStatus.Cancelled)
            return;
        
        existingItem.Cancel();
    }

    public void Update(string saleNumber, Guid customerId, string customerName)
    {
        SaleNumber = saleNumber;
        CustomerId = customerId;
        CustomerName = customerName;
        
        AddDomainEvent(new SaleUpdatedDomainEvent(Id, CustomerId));
    }
    
    private static Percentage CalculateDiscount(int quantity)
    {
        if (quantity is >= 10 and <= 20)
            return Percentage.Create(0.20m);

        if (quantity >= 4)
            return Percentage.Create(0.10m);

        return Percentage.Zero();
    }

    public void Cancel()
    {
        if (Status == SaleStatus.Cancelled)
            throw new DomainException("The sale is already cancelled.");

        Status = SaleStatus.Cancelled;

        AddDomainEvent(new SaleCancelledDomainEvent(Id));
    }
}