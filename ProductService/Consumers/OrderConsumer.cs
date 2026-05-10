using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
public class OrderConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        var connection = await factory.CreateConnectionAsync();

        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "orderQueue",
            durable: false,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (sender, e) =>
        {
            var body = e.Body.ToArray();

            var json = Encoding.UTF8.GetString(body);

            var data = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

            var product = ProductDb.Products
                .FirstOrDefault(x => x.Id == data.ProductId);

            if (product != null)
            {
                // update inventory
                product.Stock -= data.Quantity;

                Console.WriteLine($"Stock Updated: {product.Stock}");
            }

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: "orderQueue",
            autoAck: true,
            consumer: consumer);

       // return Task.CompletedTask;
    }
}