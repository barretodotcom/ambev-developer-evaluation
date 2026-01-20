using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common.Pagination;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ApiProgram = Ambev.DeveloperEvaluation.WebApi.Program;

namespace Ambev.DeveloperEvaluation.Functional.Sales.GetAllSales;

public class GetAllSalesTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public GetAllSalesTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    private async Task SeedTestData(DefaultContext db, int salesCount)
    {
        db.Sales.RemoveRange(db.Sales);
        var sales = GetAllSalesTestData.CreateManySales(salesCount);
        await db.Sales.AddRangeAsync(sales);
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllSales_ShouldReturnOrderedAndPaginated()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();

        var salesCount = 20;

        await SeedTestData(db, salesCount);

        // Act
        var response = await _client.GetAsync("/api/sales?page=2&order=customer:desc");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PaginatedResponse<GetAllSalesResponse>>();

        // Assert
        content.TotalCount.Should().Be(salesCount);
    }
}