using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleItemCommand that defines validation rules for sale item creation command.
/// </summary>
public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleItemCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// ProductId: Must be not empty
    /// ProductName: Must not be null or empty
    /// Quantity: Must be greater than 0
    /// UnitPrice: Must be greater than 0
    /// </remarks>
    public CreateSaleItemValidator()
    {
        RuleFor(l => l.ProductId).NotEmpty();
        RuleFor(l => l.ProductName)
            .NotEmpty();
        
        RuleFor(l => l.Quantity).GreaterThan(0);
        RuleFor(l => l.UnitPrice).GreaterThan(0);
    }
}