using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Functional.Sales.CancelSale;

/// <summary>
/// Provides functional integration tests for cancelling <see cref="Sale"/> entities via the API.
/// </summary>
/// <remarks>
/// This class uses a <see cref="CustomWebApplicationFactory"/> to create an in-memory
/// test host and client for functional testing. It ensures that sales cancelled via
/// the API endpoint are correctly updated in the database, including the status of
/// all associated <see cref="Domain.Entities.SaleItem"/>s.
/// </remarks>
public class CancelSaleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";
    
    public CancelSaleTests(CustomWebApplicationFactory factory)
    {
        factory.UseDatabase(_databaseName);
        _factory = factory;
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Given an existing <see cref="Sale"/> with multiple <see cref="Domain.Entities.SaleItem"/>s
    /// When a DELETE request is made to the API endpoint for that sale
    /// Then the sale and all its items are marked as <see cref="SaleStatus.Cancelled"/> and <see cref="SaleItemStatus.Cancelled"/>
    /// in the database, confirming the cancellation is fully applied.
    /// </summary>
    [Fact]
    public async Task CreateSaleSales_ShouldReturnCreatedSaleId()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();

        var sale = CancelSaleTestData.CreateValidSaleWithItems();

        db.Sales.Add(sale);
        await db.SaveChangesAsync();
        db.Entry(sale).State = EntityState.Detached;
        
        // Act
        var response = await _client.DeleteAsync($"/api/sales/{sale.Id}");
        response.EnsureSuccessStatusCode();
        
        await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        
        // Assert
        var cancelledSale = await db.Sales
            .Include(l =>l.Items)
            .FirstAsync(l => l.Id == sale.Id);

        cancelledSale.Status.Should().Be(SaleStatus.Cancelled);
        cancelledSale.Items.Count(l => l.Status == SaleItemStatus.Cancelled).Should().Be(sale.Items.Count);
    }
}