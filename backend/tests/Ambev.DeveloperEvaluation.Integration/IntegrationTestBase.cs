using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Application.Read;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.UnitOfWork;
using MediatR;

namespace Ambev.DeveloperEvaluation.Integration;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected DefaultContext DbContext = null!;
    protected IServiceProvider ServiceProvider = null!;
    protected IMediator Mediator = null!;

    public async Task InitializeAsync()
    {
        var services = new ServiceCollection();

        services.AddDbContext<DefaultContext>(options =>
            options.UseInMemoryDatabase(
                $"TestDb_{Guid.NewGuid().ToString()}"
            ));

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

        ServiceProvider = services.BuildServiceProvider();
        DbContext = ServiceProvider.GetRequiredService<DefaultContext>();
        Mediator = ServiceProvider.GetRequiredService<IMediator>();

        await Task.CompletedTask;
    }

    public Task DisposeAsync() => Task.CompletedTask;
}