namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record GetSaleItemResponse
{
    /// <summary>
    /// Gets the sale item id.
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public Guid ProductId { get; init; }
    /// <summary>
    /// Gets the product name.
    /// </summary>
    public string ProductName { get; init; } = string.Empty;
    /// <summary>
    /// Gets the product quantity.
    /// </summary>
    public int Quantity { get; init; }
    /// <summary>
    /// Gets the product unit price.
    /// </summary>
    public decimal UnitPrice { get; init; }
    /// <summary>
    /// Gets the sale item discount percentage.
    /// </summary>
    public decimal DiscountPercentage{ get; init; }

    /// <summary>
    /// Gets the sale item status.
    /// </summary>
    public string Status { get; init; } = string.Empty;
}