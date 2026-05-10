using OrderService;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/orders", async (CreateOrderRequest request) =>
{
    var factory = new ConnectionFactory()
    {
        HostName = "localhost"
    };

    using var connection = await factory.CreateConnectionAsync();

    using var channel = await connection.CreateChannelAsync();
    
    await channel.QueueDeclareAsync(
          queue: "orderQueue",
          durable: false,
          exclusive: false,
          autoDelete: false);

    var json = JsonSerializer.Serialize(request);

    var body = Encoding.UTF8.GetBytes(json);

    await channel.BasicPublishAsync(
         exchange: "",
         routingKey: "orderQueue",
         body: body);

    return Results.Ok("Order Created");
});

app.Run();

