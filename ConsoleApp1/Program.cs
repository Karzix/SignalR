using Consumer.Data;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ConsoleApp1.Model;
using MassTransit;
using Consumer;

using (var context = new ProductDbContext())
{

    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host("localhost", "/", c =>
        {
            c.Username("guest");
            c.Password("guest");
        });
        cfg.ReceiveEndpoint("product-event", e =>
        {
            e.Consumer<ProductConsumer>();
        });
    });
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}
