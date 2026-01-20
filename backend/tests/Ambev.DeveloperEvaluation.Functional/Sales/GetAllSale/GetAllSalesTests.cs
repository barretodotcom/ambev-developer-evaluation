using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Ambev.DeveloperEvaluation.Functional.Sales.GetAllSale;

public class GetAllSalesFunctionalTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetAllSalesFunctionalTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllSales_ShouldReturnOrderedAndPaginated()
    {
        // Act
        var response = await _client.GetAsync("/api/sales?page=2&order=customer:desc");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<List<GetAllSalesReadModel>>();

        // Assert
        content.Count.Should().Be(1);
        content.First().CustomerName.Should().Be("Bob");
    }
}
