namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class CancelSaleRequest
{
    /// <summary>
    /// Gets or sets the identifier of the sale to be cancelled.
    /// Must be not empty.
    /// </summary>
    public Guid Id { get; set; }
}