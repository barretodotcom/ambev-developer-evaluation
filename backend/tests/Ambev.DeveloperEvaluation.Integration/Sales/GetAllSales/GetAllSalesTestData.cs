using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Sales.GetAllSales;

public static class GetAllSalesTestData
{
    private static readonly Faker Faker = new();

    public static List<Sale> CreateManySales(int count)
    {
        var sales = new List<Sale>();

        for (int i = 0; i < count; i++)
        {
            var sale = new Sale(
                Faker.Random.Guid().ToString(),
                Guid.NewGuid(),
                Faker.Name.FullName(),
                DateTime.UtcNow.AddMinutes(-i),
                Guid.NewGuid(),
                Faker.Company.CompanyName()
            );

            sale.AddItem(
                Guid.NewGuid(),
                Faker.Commerce.ProductName(),
                Faker.Random.Int(1, 5),
                Money.Create(100)
            );

            sales.Add(sale);
        }

        return sales;
    }
}
