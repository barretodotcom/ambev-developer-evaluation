namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale item in the system.
/// </summary>
public record CreateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the product id.
    /// Must be not empty.
    /// </summary>
    public Guid ProductId { get; init; }
    /// <summary>
    /// Gets or sets the product name.
    /// Must be not empty.
    /// </summary>
    public string ProductName { get; init; } = string.Empty;
    /// <summary>
    /// Gets or sets the product quantity.
    /// Must be greater than 0.
    /// </summary>
    public int Quantity { get; init; }
    /// <summary>
    /// Gets or sets the product unit price.
    /// Must be greater than 0.
    /// </summary>
    public decimal UnitPrice { get; init; }
}