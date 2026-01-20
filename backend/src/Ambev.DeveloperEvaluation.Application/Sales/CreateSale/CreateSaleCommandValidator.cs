using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation command.
/// </summary>
public sealed class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// CustomerId: Must be not empty
    /// CustomerName: Must be not null or empty
    /// BranchId: Must be not empty
    /// BranchName: Must be not null or empty
    /// SaleDate: Must be greater than default DateTime value
    /// Items: Must not be empty
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(l => l.CustomerId).NotEmpty();
        RuleFor(l => l.CustomerName).NotEmpty();
        
        RuleFor(l => l.BranchId).NotEmpty();
        RuleFor(l => l.BranchName).NotEmpty();
        
        RuleFor(l => l.SaleDate)
            .Must(d => d > DateTime.MinValue);;
        
        RuleFor(l => l.Items).NotEmpty();
        RuleForEach(l => l.Items).SetValidator(new CreateSaleItemValidator());
    }
}