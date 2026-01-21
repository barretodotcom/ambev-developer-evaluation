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

/// <summary>
/// Provides functional tests for retrieving multiple sales via the API.
/// </summary>
/// <remarks>
/// Each test runs with an isolated in-memory database to ensure full independence,
/// preventing interference between tests and allowing parallel execution.
/// </remarks>
public class GetAllSalesTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly int MaxPagination = 100;
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";
    
    public GetAllSalesTests(CustomWebApplicationFactory factory)
    {
        factory.UseDatabase(_databaseName);
        _client = factory.CreateClient();
        _factory = factory;
    }

    private async Task SeedTestData(DefaultContext db, int salesCount)
    {
        var sales = GetAllSalesTestData.CreateManySales(salesCount);
        await db.Sales.AddRangeAsync(sales);
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Given a set of sales persisted in the database
    /// When a GET request is made to the /api/sales endpoint with pagination and ordering
    /// Then the API returns a paginated, ordered list of sales with the correct total count.
    /// </summary>
    [Fact]
    public async Task GetAllSales_ShouldReturnOrderedAndPaginated()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();

        var salesCount = 20;

        await SeedTestData(db, salesCount);

        // Act
        var response = await _client.GetAsync($"/api/sales?_size={MaxPagination}2&order=customer:desc");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<PaginatedResponse<GetAllSalesResponse>>();

        // Assert
        content.TotalCount.Should().Be(salesCount);
    }
}