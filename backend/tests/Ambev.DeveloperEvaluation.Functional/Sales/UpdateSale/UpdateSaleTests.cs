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

public class UpdateSaleIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public UpdateSaleIntegrationTests(CustomWebApplicationFactory factory)
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