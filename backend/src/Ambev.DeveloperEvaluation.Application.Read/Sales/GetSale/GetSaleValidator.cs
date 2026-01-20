using AutoMapper;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Read.Sales.GetSale;

public class GetSaleValidator : AbstractValidator<GetSaleQuery>
{
    public GetSaleValidator()
    {
        RuleFor(l => l.Id)
            .NotNull();
    }

}