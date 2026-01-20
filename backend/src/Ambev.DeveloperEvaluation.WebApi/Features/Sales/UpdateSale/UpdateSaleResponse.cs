namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Represents the response returned after successfully updating an existing sale and sale items.
/// </summary>
/// <remarks>
/// This response contains all sale items <see cref="UpdateSaleItemResponse"/>
/// </remarks>
public record UpdateSaleResponse
{
    
    /// <summary>
    /// Gets the updated sale unique identifier
    /// </summary>
    public Guid Id { get; init; }   
    /// <summary>
    /// Gets the sale number.
    /// </summary>
    public string SaleNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets the customer id.
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Gets the customer name.
    /// </summary>
    public string CustomerName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the date when the sale was made.
    /// The value must be specified and cannot be in the future.
    /// </summary>
    public DateTime SaleDate { get; init; }

    /// <summary>
    /// Gets the branch id
    /// </summary>
    public Guid BranchId { get; init; }

    /// <summary>
    /// Gets the branch name.
    /// </summary>
    public string BranchName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the date when the sale entity was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the sales items.
    /// </summary>
    public IReadOnlyList<UpdateSaleItemResponse> Items { get; init; } = new List<UpdateSaleItemResponse>();
}