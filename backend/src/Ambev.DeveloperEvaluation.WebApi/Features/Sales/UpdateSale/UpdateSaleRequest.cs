namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Represents a request to update an existing sale in the system.
/// This request can include updates to the sale details as well as the sale items.
/// All operations on sale items should be handled through <see cref="UpdateSaleItemRequest"/>.
/// </summary>
public record UpdateSaleRequest
{
    /// <summary>
    /// Gets the unique sale number.
    /// This value must not be null or empty and is used to identify the sale.
    /// </summary>
    public string SaleNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the customer associated with the sale.
    /// Must not be an empty
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Gets the name of the customer associated with the sale.
    /// Must not be null or empty.
    /// </summary>
    public string CustomerName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the date when the sale occurred.
    /// The value must be specified and cannot be in the future.
    /// </summary>
    public DateTime SaleDate { get; init; }

    /// <summary>
    /// Gets the unique identifier of the branch where the sale was made.
    /// Must not be an empty 
    /// </summary>
    public Guid BranchId { get; init; }

    /// <summary>
    /// Gets the name of the branch where the sale was made.
    /// Must not be null or empty.
    /// </summary>
    public string BranchName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the list of sale items included in this update request.
    /// Each item specifies its operation (Add, Update, Delete) through <see cref="UpdateSaleItemRequest.Operation"/>.
    /// This list can be empty if no items need to be modified.
    /// </summary>
    public List<UpdateSaleItemRequest> Items { get; set; } = new();
}
