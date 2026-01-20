namespace Ambev.DeveloperEvaluation.Application.Read.Sales.GetAllSales;
using FluentValidation;

/// <summary>
/// Validator for GetAllSalesQuery that defines validation rules for sale item creation command.
/// </summary>
public class GetAllSalesValidator : AbstractValidator<GetAllSalesQuery>
{
    /// <summary>
    /// Initializes a new instance of the GetAllSalesCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// Page: Must be greater than zero
    /// PageSize: Must be greater than zero
    /// </remarks>
    public GetAllSalesValidator()
    {
        RuleFor(l => l.Page)
            .GreaterThan(0);

        RuleFor(l => l.PageSize)
            .GreaterThan(0);
    }
}