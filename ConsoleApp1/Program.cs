using Consumer.Data;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ConsoleApp1.Model;

using (var context = new ProductDbContext())
{
    var factory = new ConnectionFactory() { HostName = "localhost" };
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    channel.QueueDeclare(queue: "productQueue",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var hubConnection = new HubConnectionBuilder()
        .WithUrl("https://localhost:7020/notifications")
        .Build();

    await hubConnection.StartAsync();

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += async (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var product = JsonSerializer.Deserialize<Product>(message);
        product.Id = Guid.NewGuid();
        context.Products.Add(product);
        await context.SaveChangesAsync();

        await hubConnection.InvokeAsync("SendMessage", "Product saved: " + product.Name);
    };

    channel.BasicConsume(queue: "productQueue",
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}
