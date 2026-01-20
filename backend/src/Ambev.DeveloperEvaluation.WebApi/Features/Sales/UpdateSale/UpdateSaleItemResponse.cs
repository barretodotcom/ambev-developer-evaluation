namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public record UpdateSaleItemResponse
{
    /// <summary>
    /// Gets or sets the sale item id.
    /// </summary>
    public Guid Id { get; private set; }

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
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets or sets the sale item status
    /// default 'Active'
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Gets or sets the discount percentage
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Gets or sets the product unit price
    /// </summary>
    public decimal TotalAmount { get; init; }
    
}