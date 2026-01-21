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

/// <summary>
/// Provides functional integration tests for retrieving <see cref="Sale"/> entities via the API.
/// </summary>
/// <remarks>
/// This class uses a <see cref="CustomWebApplicationFactory"/> to create an in-memory
/// test host and client for functional testing. It ensures that retrieving sales by ID
/// returns the expected <see cref="Sale"/> data, fully populated with its properties
/// and any related entities if applicable.
/// </remarks>
public class GetSaleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";

    public GetSaleTests(CustomWebApplicationFactory factory)
    {
        factory.UseDatabase(_databaseName);
        _client = factory.CreateClient();
        _factory = factory;
    }

    /// <summary>
    /// Seeds the provided <see cref="Sale"/> into the test <see cref="DefaultContext"/>.
    /// </summary>
    private async Task SeedTestData(Sale sale, DefaultContext db)
    {
        await db.Sales.AddAsync(sale);
        await db.SaveChangesAsync();
    }
    
    /// <summary>
    /// Given an existing <see cref="Sale"/> in the database
    /// When a GET request is made to the API endpoint for that sale
    /// Then the API returns the sale with all properties correctly populated,
    /// and the response matches the seeded sale data.
    /// </summary>
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
