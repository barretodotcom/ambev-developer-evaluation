using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.CreateSale;

public class CreateSaleTestData
{
    private static readonly Faker<CreateSaleItemCommand> SaleItemFaker =
        new Faker<CreateSaleItemCommand>()
            .RuleFor(i => i.ProductId, f => f.Random.Guid())
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 500));

    private static readonly Faker<CreateSaleCommand> SaleCommandFaker =
        new Faker<CreateSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => f.Random.Replace("SALE-#####"))
            .RuleFor(s => s.CustomerId, f => f.Random.Guid())
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.SaleDate, f => f.Date.Recent())
            .RuleFor(s => s.BranchId, f => f.Random.Guid())
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => SaleItemFaker.Generate(3));

    public static CreateSaleCommand GenerateValidCommand()
    {
        return SaleCommandFaker.Generate();
    }
}