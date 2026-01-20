using Ambev.DeveloperEvaluation.WebApi.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Represents a request to create/update/cancel a sale item in the system.
/// </summary>
public class UpdateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the item unique identifier.
    /// Can be null if ItemOperation equals to 'Create'
    /// </summary>
    public Guid? Id { get; set; }
    
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
    public UpdateSaleItemOperation Operation { get; set; }
}