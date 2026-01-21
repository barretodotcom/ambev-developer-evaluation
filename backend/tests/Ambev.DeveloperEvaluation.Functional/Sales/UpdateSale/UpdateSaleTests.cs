using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Enums;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using SaleItemStatus = Ambev.DeveloperEvaluation.Domain.Enums.SaleItemStatus;

namespace Ambev.DeveloperEvaluation.Functional.Sales.UpdateSale;

/// <summary>
/// Provides functional integration tests for updating <see cref="Sale"/> entities via the Web API.
/// </summary>
/// <remarks>
/// This class uses a <see cref="CustomWebApplicationFactory"/> to create an in-memory
/// test host and client for functional testing. It ensures that update operations on
/// <see cref="Sale"/> and their associated <see cref="Domain.Entities.SaleItem"/>s
/// are correctly applied, persisted, and returned by the API.
/// </remarks>
public class UpdateSaleIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";

    public UpdateSaleIntegrationTests(CustomWebApplicationFactory factory)
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
    /// Given an existing <see cref="Sale"/> with multiple <see cref="Domain.Entities.SaleItem"/>s
    /// When a <see cref="UpdateSaleCommand"/> is sent via the API endpoint
    /// Then the sale and its items are updated correctly, including item operations
    /// such as update, creation, and cancellation, and changes are persisted in the database.
    /// </summary>
    [Fact]
    public async Task Should_Update_Sale_And_Items_Correctly()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();

        var sale = UpdateSaleTestData.CreateValidSaleWithItems();
        await SeedTestData(sale, db);
        db.Entry(sale).State = EntityState.Detached;

        var command = UpdateSaleTestData.GenerateValidCommandFromExistingSale(sale);
        // Act
        var response = await _client.PutAsJsonAsync($"/api/sales/{sale.Id}", command);
        response.EnsureSuccessStatusCode();

        await response.Content.ReadFromJsonAsync<ApiResponseWithData<UpdateSaleResult>>();

        var updatedSale = db.Sales
            .Include(l => l.Items)
            .First(l => l.Id == sale.Id);
        
        updatedSale.SaleNumber.Should().Be(command.SaleNumber);
        updatedSale.CustomerId.Should().Be(command.CustomerId);
        updatedSale.CustomerName.Should().Be(command.CustomerName);
        updatedSale.Items.Count.Should().Be(3);

        updatedSale.Items.Count(l => l.Status == SaleItemStatus.Cancelled).Should().Be(1);
        updatedSale.Items.Count(l => l.Status == SaleItemStatus.Active).Should().Be(2);
    }
}