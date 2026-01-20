using Ambev.DeveloperEvaluation.Application.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Represents a command to create a new sale in the system.
/// </summary>
public record UpdateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the item unique identifier.
    /// Can be null if ItemOperation equals to 'Create'
    /// </summary>
    public Guid? Id { get;init; }
    
    /// <summary>
    /// Gets or sets the product id.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the products quantity.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Gets or sets the product unit price.
    /// </summary>
    public decimal UnitPrice { get; init; }
    /// <summary>
    /// Represents a request to create a new sale in the system.
    /// </summary>
    public UpdateSaleItemOperation Operation { get;init; }
}