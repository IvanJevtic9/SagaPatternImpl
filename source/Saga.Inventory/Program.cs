using RabbitMQ.Client;
using Saga.Common.IoC;
using Saga.Common.RabbitMQ.Abstraction;
using Saga.Common.Settings;
using Saga.Inventory.Data;
using Saga.Inventory.Messaging;
using SagaImpl.InventoryService.HostingServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddDatabaseContext<InventoryDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddRabbitMQ(builder.Configuration);
builder.Services.AddScoped((serviceProvider) =>
{
    var connectionProvider = serviceProvider.GetRequiredService<IConnectionProvider>();

    return new InventoryPublisher(
        connectionProvider,
        "Inventory.Exchange",
        ExchangeType.Topic
    );
});
builder.Services.AddScoped((serviceProvider) =>
{
    var connectionProvider = serviceProvider.GetRequiredService<IConnectionProvider>();

    return new OrderSubscriber(
        connectionProvider,
        "Order.Exchange",
        "Inventory.ReserveItem",
        ExchangeType.Topic
    );
});

builder.Services.AddHostedService<OrderListener>();

var app = builder.Build();
// Configure the HTTP request pipeline.
var swaggerSettings = app.Services.GetRequiredService<SwaggerSettings>();

await PrepDb.PopulateDatabase(app);

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