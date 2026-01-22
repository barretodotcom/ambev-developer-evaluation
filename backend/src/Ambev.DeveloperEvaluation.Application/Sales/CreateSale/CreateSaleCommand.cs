using Ambev.DeveloperEvaluation.Application.Abstractions.Commands;
using MediatR;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale,
/// including customerId, saleDate, saleNumber, items,
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CreateSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CreateSaleCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public record CreateSaleCommand : ICommand<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the customer id.
    /// </summary>
    public Guid CustomerId { get; init; }
    /// <summary>
    /// Gets or sets the sale number.
    /// Must not be null or empty.
    /// </summary>
    public string SaleNumber { get; init; } = string.Empty;
    /// <summary>
    /// Gets or sets the sale date.
    /// </summary>
    public DateTime SaleDate { get; init; }
    /// <summary>
    /// Gets or sets the branch id
    /// </summary>
    public Guid BranchId { get; init; }
    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; init; } = string.Empty;
    /// <summary>
    /// Gets or sets the sale items.
    /// </summary>
    public IReadOnlyCollection<CreateSaleItemCommand> Items { get; init; } = Array.Empty<CreateSaleItemCommand>();
}