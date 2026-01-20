using MediatR;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Command for cancelling a sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for cancelling a sale,
/// including customerId, saleDate, saleNumber, items,
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// 
/// The data provided in this command is validated using the 
/// <see cref="CancelSaleCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public record CancelSaleCommand : IRequest
{
    /// <summary>
    /// Gets or sets the identifier of the sale to be cancelled.
    /// Must be not empty.
    /// </summary>
    public Guid Id { get; init; }   
}