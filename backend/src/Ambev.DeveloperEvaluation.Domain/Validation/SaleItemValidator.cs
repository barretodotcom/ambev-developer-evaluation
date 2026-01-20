using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public sealed class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty();

        RuleFor(item => item.Quantity)
            .GreaterThan(0);

        RuleFor(item => item.UnitPrice.Value)
            .GreaterThan(0);
    }
}
