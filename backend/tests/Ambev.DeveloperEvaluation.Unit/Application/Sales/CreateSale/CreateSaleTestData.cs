using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale;

/// <summary>
/// Provides methods for generating test data for the Create Sale use case using Bogus.
/// Centralizes the creation of valid sale commands and items to ensure consistency
/// across application layer unit tests.
/// </summary>
public static class CreateSaleTestData
{
    /// <summary>
    /// Faker configuration for generating valid sale item commands.
    /// Generated items contain:
    /// - ProductId (random GUID)
    /// - ProductName (commerce product name)
    /// - Quantity (between 1 and 10)
    /// - UnitPrice (between 10 and 500)
    /// </summary>
    private static readonly Faker<CreateSaleItemCommand> SaleItemFaker =
        new Faker<CreateSaleItemCommand>()
            .RuleFor(i => i.ProductId, f => f.Random.Guid())
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 500));

    /// <summary>
    /// Faker configuration for generating valid create sale commands.
    /// Generated commands contain:
    /// - Sale and customer identification data
    /// - Branch information
    /// - A list of valid sale items
    /// </summary>
    private static readonly Faker<CreateSaleCommand> SaleCommandFaker =
        new Faker<CreateSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => f.Random.Replace("SALE-#####"))
            .RuleFor(s => s.CustomerId, f => f.Random.Guid())
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.SaleDate, f => f.Date.Recent())
            .RuleFor(s => s.BranchId, f => f.Random.Guid())
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, _ => SaleItemFaker.Generate(3));

    /// <summary>
    /// Generates a valid <see cref="CreateSaleCommand"/> with items.
    /// </summary>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return SaleCommandFaker.Generate();
    }
}