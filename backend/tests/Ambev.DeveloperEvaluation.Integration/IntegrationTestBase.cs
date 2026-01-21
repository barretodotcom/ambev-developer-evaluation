using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Abstractions.Events;
using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Application.Read;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Messaging.Outbox;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Interceptors;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.UnitOfWork;
using MediatR;

namespace Ambev.DeveloperEvaluation.Integration;

/// <summary>
/// Base class for integration tests.
/// Provides a fully configured dependency injection container,
/// in-memory database context, and MediatR pipeline to simulate
/// the application's runtime behavior in integration scenarios.
/// </summary>
/// <remarks>
/// This base class is responsible for:
/// - Configuring an in-memory database for isolation between tests
/// - Registering application and read-layer dependencies
/// - Enabling domain events via EF Core interceptors
/// - Managing the lifecycle of the service provider and its resources
/// </remarks>
public abstract class IntegrationTestBase : IAsyncLifetime
{
    /// <summary>
    /// Exposes the service provider to derived integration tests,
    /// allowing resolution of registered services.
    /// </summary>
    protected IServiceProvider ServiceProvider = null!;

    /// <summary>
    /// Provides access to MediatR for sending commands and queries
    /// during integration test execution.
    /// </summary>
    protected IMediator Mediator = null!;

    /// <summary>
    /// Provides direct access to the EF Core database context
    /// configured for integration tests.
    /// </summary>
    protected DefaultContext DbContext = null!;

    /// <summary>
    /// Initializes the integration test environment.
    /// Builds the dependency injection container, configures the in-memory
    /// database with domain event interception, and resolves core services.
    /// </summary>
    public async Task InitializeAsync()
    {
        var services = new ServiceCollection();

        // InMemory database is used for integration tests simplicity.
        // It does not enforce relational constraints like a real database.
        services.AddDbContext<DefaultContext>((sp, options) =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
                .AddInterceptors(new DomainEventsInterceptor(mediator));
        });

        services.AddAutoMapper(
            typeof(ApplicationLayer).Assembly,
            typeof(ApplicationReadLayer).Assembly
        );

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(ApplicationReadLayer).Assembly);
        });

        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IIntegrationEventsDispatcher, OutboxWriter>();

        services.AddLogging();

        ServiceProvider = services.BuildServiceProvider();

        DbContext = ServiceProvider.GetRequiredService<DefaultContext>();
        Mediator = ServiceProvider.GetRequiredService<IMediator>();

        await Task.CompletedTask;
    }

    /// <summary>
    /// Disposes all resources created for the integration test execution.
    /// Ensures proper cleanup of the dependency injection container,
    /// database context, and any scoped or singleton services.
    /// </summary>
    public async Task DisposeAsync()
    {
        if (ServiceProvider is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();
    }
}