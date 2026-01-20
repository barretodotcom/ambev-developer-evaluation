using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Validator for CancelSaleCommand that defines validation rules for sale cancellation command.
/// </summary>
public sealed class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// Id: Must be not empty
    /// </remarks>
    public CancelSaleCommandValidator()
    {
        RuleFor(l => l.Id).NotEmpty();
    }
}