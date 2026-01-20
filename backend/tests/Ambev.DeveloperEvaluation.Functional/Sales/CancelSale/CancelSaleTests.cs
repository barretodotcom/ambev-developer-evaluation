using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Common.Pagination;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiProgram = Ambev.DeveloperEvaluation.WebApi.Program;

namespace Ambev.DeveloperEvaluation.Functional.Sales.CancelSale;

public class CancelSaleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public CancelSaleTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }


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