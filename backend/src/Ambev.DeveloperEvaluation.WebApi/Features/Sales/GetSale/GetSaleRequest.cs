using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Represents a request to get a unique existing sale.
/// </summary>
public record GetSaleRequest
{
    /// <summary>
    /// Gets or sets the identifier of the sale.
    /// Must be not empty.
    /// </summary>
    
    public Guid Id { get; init; }

}