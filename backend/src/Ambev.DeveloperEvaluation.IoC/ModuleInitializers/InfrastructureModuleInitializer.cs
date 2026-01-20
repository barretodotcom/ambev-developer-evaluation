using Ambev.DeveloperEvaluation.Application.Abstractions.Transactions;
using Ambev.DeveloperEvaluation.Application.Abstractions.Events;
using Ambev.DeveloperEvaluation.Application.Read.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Messaging.EventBus;
using Ambev.DeveloperEvaluation.Messaging.Outbox;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.ReadModel;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.Transactions;
using Ambev.DeveloperEvaluation.ORM.UnitOfWork;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddScoped<ISaleReadRepository, SaleReadRepository>();
        
        builder.Services.AddScoped<ITransactionManager, EfCoreTransactionManager>();
        builder.Services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
        
        builder.Services.AddScoped<IIntegrationEventsDispatcher, OutboxWriter>();

        builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
        builder.Services.AddSingleton<IEventBusPublisher, ServiceBusPublisher>();
        
        var connectionString = builder.Configuration.GetValue<string>("Azure:ServiceBus:ConnectionString");

        builder.Services.AddSingleton(new ServiceBusClient(connectionString));
    }
}