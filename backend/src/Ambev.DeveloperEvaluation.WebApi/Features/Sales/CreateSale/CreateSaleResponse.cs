namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents the response returned after successfully creating a new sale.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the newly created sale.
/// </remarks>
public class CreateSaleResponse
{
    /// <summary>
    /// Represents the created sale id
    /// </summary>
    public Guid Id { get; set; }
}