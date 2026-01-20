using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSale;

public static class GetSaleTestData
{
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