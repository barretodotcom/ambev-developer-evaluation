using System.Security.Claims;
using Ambev.DeveloperEvaluation.Functional.Auth;
using Ambev.DeveloperEvaluation.ORM;
using Bogus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiProgram = Ambev.DeveloperEvaluation.WebApi.Program;

namespace Ambev.DeveloperEvaluation.Functional;

/// <summary>
/// Provides a custom <see cref="WebApplicationFactory{TEntryPoint}"/> for functional tests
/// of the Web API.
/// </summary>
/// <remarks>
/// Configures:
/// - In-memory database for isolation
/// - Default fake authentication
/// </remarks>
public class CustomWebApplicationFactory : WebApplicationFactory<ApiProgram>
{
    private string DatabaseName = string.Empty;
    private ClaimsPrincipal? _testUser;
    private static readonly Faker Faker = new();

    /// <summary>
    /// Sets the database name for the in-memory database.
    /// </summary>
    public void UseDatabase(string databaseName)
    {
        DatabaseName = databaseName;
    }

    /// <summary>
    /// Sets a fake authenticated user for all tests. Uses a default if none is provided.
    /// </summary>
    public void UseTestAuthentication(ClaimsPrincipal? user = null)
    {
        _testUser = user ?? GetDefaultTestUser();
    }

    /// <summary>
    /// Returns a default fake authenticated user with SalesManager role.
    /// </summary>
    private ClaimsPrincipal GetDefaultTestUser()
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "SalesManager")
        }, "Test"));
    }

    /// <summary>
    /// Configures the WebHost for tests:
    /// - In-memory DbContext
    /// - Authentication with TestAuthHandler
    /// - Database initialization
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove real DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            if (string.IsNullOrEmpty(DatabaseName))
                throw new InvalidOperationException("No database name specified.");

            // InMemory DbContext
            services.AddDbContext<DefaultContext>(options =>
                options.UseInMemoryDatabase(DatabaseName));

            // Authentication fake
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            // Register the user
            if (_testUser != null)
                services.AddSingleton(_testUser);

            // Initialize database
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            db.Database.EnsureCreated();
        });
    }
}
