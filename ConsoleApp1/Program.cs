using Consumer.Data;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ConsoleApp1.Model;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "fileQueue",
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
    var fileMessage = JsonSerializer.Deserialize<FileMessage>(message);

    if (fileMessage != null && fileMessage.FileContent != null)
    {
        using (var memoryStream = new MemoryStream(fileMessage.FileContent))
        using (var reader = new StreamReader(memoryStream))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                // Optionally, send the line to SignalR hub
                await hubConnection.InvokeAsync("SendMessage", line);
            }
        }
    }
};

channel.BasicConsume(queue: "file",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

public class FileMessage
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}
