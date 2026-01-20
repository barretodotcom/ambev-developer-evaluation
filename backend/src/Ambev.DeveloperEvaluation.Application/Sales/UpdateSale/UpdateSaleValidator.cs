using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleValidator that defines validation rules for sale item creation command.
/// </summary>
public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateSaleValidatorValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// ProductId: Must be not empty
    /// ProductName: Must not be null or empty
    /// CustomerId: Must be not empty
    /// CustomerName: Must not be null or empty
    /// SaleNumber: Must not be null or empty
    /// </remarks>
    public UpdateSaleValidator()
    {
        RuleFor(l => l.Id)
            .NotEmpty();
        RuleFor(l => l.SaleNumber)
            .NotEmpty();
        RuleFor(l => l.CustomerId)
            .NotEmpty();
        RuleFor(l => l.CustomerName)
            .NotEmpty();
        RuleForEach(l => l.Items)
            .SetValidator(new UpdateSaleItemValidator());
    }
}