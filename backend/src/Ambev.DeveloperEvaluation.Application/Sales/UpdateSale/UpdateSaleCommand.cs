using Ambev.DeveloperEvaluation.Application.Abstractions.Commands;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Represents a command to update an existing sale in the system.
/// This command can include updates to the sale details as well as the sale items.
/// All operations on sale items should be handled through <see cref="UpdateSaleItemCommand"/>.
/// </summary>
public record UpdateSaleCommand : ICommand<UpdateSaleResult>
{
    /// <summary>
    /// Gets the unique sale identifier.
    /// This value must not be null or empty and is used to identify the sale.
    /// </summary>
    public Guid Id { get; set; }
    
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
    /// Gets the list of sale items included in this update request.
    /// Each item specifies its operation (Add, Update, Delete) through <see cref="UpdateSaleItemCommand.Operation"/>.
    /// This list can be empty if no items need to be modified.
    /// </summary>
    public List<UpdateSaleItemCommand> Items { get; init; } = new();
}