namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public sealed record CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the customer id.
    /// Must be not empty.
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Gets or sets the sale number
    /// Must be not empty and less than or equal current number.
    /// </summary>
    public string SaleNumber { get; init; } = string.Empty;
    /// <summary>
    /// Gets or sets the date the sale was made.
    /// Must be not empty and less than or equal current date.
    /// </summary>
    public DateTime SaleDate { get; init; }
    /// <summary>
    /// Gets or sets the branch id
    /// Must be not empty.
    /// </summary>
    public Guid BranchId { get; set; }
    /// <summary>
    /// Gets or sets the branch name.
    /// Must be not empty.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the sale items.
    /// Must be not empty.
    /// </summary>
    public IReadOnlyCollection<CreateSaleItemRequest> Items { get; init; } = Array.Empty<CreateSaleItemRequest>();
}