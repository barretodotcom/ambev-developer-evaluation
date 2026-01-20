namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record GetSaleResponse
{
    /// <summary>
    /// Gets the sale unique identifier.
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
    /// </summary>
    public DateTime SaleDate { get; init; }
    /// <summary>
    /// Gets the sale status.
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Gets the sale items
    /// <see cref="GetSaleItemResponse"/>
    /// </summary>
    public IReadOnlyList<GetSaleItemResponse> Items { get; init; } = new List<GetSaleItemResponse>();
}