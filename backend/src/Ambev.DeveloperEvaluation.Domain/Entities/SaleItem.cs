using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public sealed class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the product id.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets or sets the products quantity.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets or sets the product unit price.
    /// </summary>
    public Money UnitPrice { get; private set; }

    /// <summary>
    /// Gets or sets the sale item status
    /// default 'Active'
    /// </summary>
    public SaleItemStatus Status { get; private set; }

    /// <summary>
    /// Gets or sets the discount percentage
    /// </summary>
    public Percentage DiscountPercentage { get; private set; }

    /// <summary>
    /// Gets or sets the product unit price
    /// </summary>
    public Money TotalAmount => Money.Create(UnitPrice.Value * Quantity);

    /// <summary>
    /// Gets the date and time when the sale item was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    private SaleItem()
    {
    }

    public SaleItem(Guid productId, string productName, int quantity, Money unitPrice, Percentage discountPercentage)
    {
        Id = Guid.NewGuid();
        
        ProductId = productId;
        ProductName = productName;
        
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercentage = discountPercentage;

        Status = SaleItemStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateQuantityAndDiscount(int quantity, Percentage discountPercentage)
    {
        Quantity += quantity;
        DiscountPercentage = discountPercentage;
    }
}