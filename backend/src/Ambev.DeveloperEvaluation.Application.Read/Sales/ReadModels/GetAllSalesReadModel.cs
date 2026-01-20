namespace Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;

public record GetAllSalesReadModel
{
    /// <summary>
    /// Gets the sale unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets the customer id.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets the sale status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets the total items' quantity.
    /// </summary>
    public int ItemsQuantity;

    /// <summary>
    /// Gets the total amount.
    /// </summary>
    public decimal TotalAmount;

}