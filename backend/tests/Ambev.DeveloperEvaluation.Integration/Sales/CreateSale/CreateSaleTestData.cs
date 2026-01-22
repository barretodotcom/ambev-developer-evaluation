using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.CreateSale;

/// <summary>
/// Provides factory methods for generating test data
/// for Create Sale integration tests.
/// </summary>
/// <remarks>
/// This class centralizes the creation of valid <see cref="CreateSaleCommand"/> instances
/// using Bogus to ensure realistic, consistent, and repeatable test scenarios.
/// </remarks>
public class CreateSaleTestData
{
    /// <summary>
    /// Faker configuration responsible for generating valid sale item commands.
    /// </summary>
    /// <remarks>
    /// Each generated item includes:
    /// <list type="bullet">
    /// <item>Product identifier and descriptive name</item>
    /// <item>Quantity greater than zero</item>
    /// <item>Valid unit price</item>
    /// </list>
    /// </remarks>
    private static readonly Faker<CreateSaleItemCommand> SaleItemFaker =
        new Faker<CreateSaleItemCommand>()
            .RuleFor(i => i.ProductId, f => f.Random.Guid())
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 500));

    /// <summary>
    /// Faker configuration responsible for generating valid
    /// <see cref="CreateSaleCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// The generated command represents a complete sale creation request,
    /// including customer information, branch details, sale date,
    /// and multiple sale items.
    /// </remarks>
    private static readonly Faker<CreateSaleCommand> SaleCommandFaker =
        new Faker<CreateSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => f.Random.Replace("SALE-#####"))
            .RuleFor(s => s.SaleDate, f => f.Date.Recent())
            .RuleFor(s => s.BranchId, f => f.Random.Guid())
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => SaleItemFaker.Generate(3));

    public static CreateSaleCommand GenerateValidCommand(Guid customerId)
    {
        var command =  SaleCommandFaker.RuleFor(l => l.CustomerId, _ => customerId).Generate();
        
        return command;
    }

    public static User GenerateValidUser()
    {
        return new Faker<User>()
            .RuleFor(l => l.Id, f => f.Random.Guid())
            .RuleFor(l => l.Username, f => f.Person.FullName);
    }
}