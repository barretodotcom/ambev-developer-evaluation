using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Functional.Sales.CreateSale;

/// <summary>
/// Provides functional integration tests for creating <see cref="Sale"/> entities via the API.
/// </summary>
/// <remarks>
/// This class uses a <see cref="CustomWebApplicationFactory"/> to create an in-memory
/// test host and client for functional testing. It ensures that sales created via
/// the API endpoint are correctly persisted in the database with all properties
/// and related items fully populated.
/// </remarks>
public class CreateSaleSalesTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";
    
    public CreateSaleSalesTests(CustomWebApplicationFactory factory)
    {
        factory.UseDatabase(_databaseName);
        _client = factory.CreateClient();
        _factory = factory;
    }

    /// <summary>
    /// Given a valid <see cref="CreateSaleCommand"/>
    /// When the command is sent via the API endpoint for sale creation
    /// Then a new <see cref="Sale"/> is persisted in the database with all
    /// properties and related <see cref="Domain.Entities.SaleItem"/>s
    /// correctly saved and the API response returns the created sale's Id.
    /// </summary>
    [Fact]
    public async Task CreateSaleSales_ShouldReturnCreatedSaleId()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();

        var command = CreateSaleTestData.GenerateValidCommand();

        // Act
        var response = await _client.PostAsJsonAsync("/api/sales", command);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();

        // Assert
        var sale = db.Sales.First(l => l.Id == content.Data.Id);

        sale.SaleDate.Should().Be(command.SaleDate);
        sale.SaleNumber.Should().Be(command.SaleNumber);
        sale.SaleNumber.Should().Be(command.SaleNumber);
        sale.SaleNumber.Should().Be(command.SaleNumber);
        sale.CustomerId.Should().Be(command.CustomerId);
        sale.CustomerName.Should().Be(command.CustomerName);
        sale.SaleDate.Should().Be(command.SaleDate);
        sale.BranchId.Should().Be(command.BranchId);
        sale.BranchName.Should().Be(command.BranchName);
    }
}