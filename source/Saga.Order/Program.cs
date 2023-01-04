using FluentValidation;
using FluentValidation.AspNetCore;
using RabbitMQ.Client;
using Saga.Common.IoC;
using Saga.Common.Messaging;
using Saga.Common.Messaging.Abstraction;
using Saga.Common.RabbitMQ.Abstraction;
using Saga.Common.Saga.Abstraction;
using Saga.Common.Settings;
using Saga.Order.Data;
using Saga.Order.Dtos;
using Saga.Order.Messaging;
using Saga.Order.Saga;
using Saga.Order.Validation;
using SagaImpl.InventoryService.HostingServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddDatabaseContext<OrderDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddScoped<UnitOfWork>();

/* Add Validator */
builder.Services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderValidation>();

/* Event processing and Saga*/
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddScoped<ISaga, CreateOrderSaga>();

/* Saga Orchestration*/
builder.Services.AddScoped<IOrchestration, SagaOrchestration>((serviceProvider) =>
{
    var unitOfWork = serviceProvider.GetRequiredService<UnitOfWork>();
    return new SagaOrchestration(unitOfWork);
});

builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddScoped((serviceProvider) =>
{
    var connectionProvider = serviceProvider.GetRequiredService<IConnectionProvider>();

    return new OrderPublisher(
        connectionProvider,
        "Order.Exchange",
        ExchangeType.Topic
    );
});
builder.Services.AddScoped((serviceProvider) =>
{
    var connectionProvider = serviceProvider.GetRequiredService<IConnectionProvider>();

    return new InventorySubscriber(
        connectionProvider,
        "Inventory.Exchange",
        "Order.ReservedItem",
        ExchangeType.Topic
    );
});

builder.Services.AddHostedService<InventoryListener>();

var app = builder.Build();
// Configure the HTTP request pipeline.
var swaggerSettings = app.Services.GetRequiredService<SwaggerSettings>();

app.UseExceptionMiddelware();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(swaggerSettings.JsonRoute, $"{swaggerSettings.Title} {swaggerSettings.Version}");
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
