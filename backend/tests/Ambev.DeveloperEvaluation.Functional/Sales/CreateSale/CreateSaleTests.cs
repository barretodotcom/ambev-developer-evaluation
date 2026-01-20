using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Common.Pagination;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ApiProgram = Ambev.DeveloperEvaluation.WebApi.Program;

namespace Ambev.DeveloperEvaluation.Functional.Sales.CreateSale;

public class CreateSaleSalesTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public CreateSaleSalesTests(CustomWebApplicationFactory factory)
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