using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Read.Sales.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.ORM;
using Bogus;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiProgram = Ambev.DeveloperEvaluation.WebApi.Program;

namespace Ambev.DeveloperEvaluation.Functional;

/// <summary>
/// Provides a custom <see cref="WebApplicationFactory{TEntryPoint}"/> for functional tests
/// of the Web API.
/// </summary>
/// <remarks>
/// This class configures the application host for functional testing by:
/// - Replacing the real database context with an in-memory database (<see cref="DefaultContext"/>)
/// - Ensuring the database is created and ready before each test
/// It enables tests to run in isolation without affecting real data or requiring external dependencies.
/// </remarks>
public class CustomWebApplicationFactory : WebApplicationFactory<ApiProgram>
{
    private string DatabaseName = string.Empty;
    private static readonly Faker Faker = new();

    public void UseDatabase(string databaseName)
    {
        DatabaseName = databaseName;
    }

    /// <summary>
    /// Given a Web API host builder
    /// When configuring the web host for functional testing
    /// Then the real <see cref="DbContextOptions{DefaultContext}"/> is replaced
    /// with an in-memory database and the database is initialized.
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            if (string.IsNullOrEmpty(DatabaseName))
                throw new InvalidOperationException("No database name specified.");
            
            services.AddDbContext<DefaultContext>(options =>
            {
                options.UseInMemoryDatabase(DatabaseName);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            db.Database.EnsureCreated();
        });
    }
}