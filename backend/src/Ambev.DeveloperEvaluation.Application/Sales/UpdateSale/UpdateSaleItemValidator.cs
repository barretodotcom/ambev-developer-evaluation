using Ambev.DeveloperEvaluation.Application.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleItemValidator that defines validation rules for sale item creation command.
/// </summary>
public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleItemValidatorValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// ProductId: Must be not empty
    /// ProductName: Must not be null or empty
    /// Quantity: Must be greater than 0
    /// UnitPrice: Must be greater than 0
    /// </remarks>
    public UpdateSaleItemValidator()
    {
        RuleFor(l => l.Id)
            .Null()
            .When(l => l.Operation == UpdateSaleItemOperation.Create);
        
        RuleFor(l => l.Id)
            .NotNull()
            .When(l => l.Operation is UpdateSaleItemOperation.Cancel or UpdateSaleItemOperation.Update);
        
        RuleFor(l => l.ProductId)
            .NotEmpty();
        RuleFor(l => l.ProductName)
            .NotEmpty();
        RuleFor(l => l.Quantity)
            .GreaterThan(0);
        RuleFor(l => l.UnitPrice)
            .GreaterThan(0);
    }
}