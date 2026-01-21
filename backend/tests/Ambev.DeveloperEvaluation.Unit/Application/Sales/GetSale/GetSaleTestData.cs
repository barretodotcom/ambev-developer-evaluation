using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

/// <summary>
/// Provides methods for generating test data for the Get Sale read query.
/// Centralizes the creation of read models with configurable identifiers
/// and item quantities to support application-layer unit tests.
/// </summary>
public static class GetSaleTestData
{
    /// <summary>
    /// Generates a <see cref="GetSaleReadModel"/> with a configurable sale identifier
    /// and number of items.
    /// </summary>
    /// <param name="saleId">
    /// Optional sale identifier. If not provided, a new <see cref="Guid"/> is generated.
    /// </param>
    /// <param name="itemsCount">
    /// The number of sale items to generate.
    /// </param>
    /// <returns>
    /// A populated <see cref="GetSaleReadModel"/> instance.
    /// </returns>
    public static GetSaleReadModel CreateReadModel(Guid? saleId = null, int itemsCount = 2)
    {
        var itemFaker = new Faker<GetSaleItemReadModel>()
            .RuleFor(i => i.Id, _ => Guid.NewGuid())
            .RuleFor(i => i.ProductId, _ => Guid.NewGuid())
            .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 5))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 100))
            .RuleFor(i => i.Status, f => f.PickRandom("Active", "Cancelled"));

        var items = itemFaker.Generate(itemsCount);

        var saleFaker = new Faker<GetSaleReadModel>()
            .RuleFor(s => s.Id, _ => saleId ?? Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(s => s.CustomerId, _ => Guid.NewGuid())
            .RuleFor(s => s.CustomerName, f => f.Person.FullName)
            .RuleFor(s => s.SaleDate, f => f.Date.Past())
            .RuleFor(s => s.Status, f => f.PickRandom("Active", "Cancelled"))
            .RuleFor(s => s.Items, _ => items);

        return saleFaker.Generate();
    }
}