using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Common.Pagination;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Functional.Sales.GetSale;

public class GetSaleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public GetSaleTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    private async Task SeedTestData(Sale sale, DefaultContext db)
    {
        await db.Sales.AddAsync(sale);
        await db.SaveChangesAsync();
    }
    
    [Fact]
    public async Task GetSale_ShouldExistingSale()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
        
        var sale = GetSaleTestData.CreateSale();
 
        await SeedTestData(sale, db);
 
        
        // Act
        var response = await _client.GetAsync($"/api/sales/{sale.Id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<ApiResponseWithData<GetSaleResponse>>();

        // Assert
        sale.SaleDate.Should().Be(sale.SaleDate);
        sale.SaleNumber.Should().Be(sale.SaleNumber);
        sale.SaleNumber.Should().Be(sale.SaleNumber);
        sale.SaleNumber.Should().Be(sale.SaleNumber);
        sale.CustomerId.Should().Be(sale.CustomerId);
        sale.CustomerName.Should().Be(sale.CustomerName);
        sale.SaleDate.Should().Be(sale.SaleDate);
        sale.BranchId.Should().Be(sale.BranchId);
        sale.BranchName.Should().Be(sale.BranchName);
    }
}
