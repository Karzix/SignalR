using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Model;
using RabbitMQ.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IModel _channel;
        //private readonly HubConnection _hubConnection;

        public ProductController(IModel channel)
        {
            _channel = channel;
            //_hubConnection = new HubConnectionBuilder()
            //    .WithUrl("https://localhost:7020/notifications")
            //    .Build();
            //_hubConnection.StartAsync().Wait();
        }

        [HttpPost]
        public IActionResult Post(Product product)
        {
            var message = JsonSerializer.Serialize(product);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "productQueue",
                                  basicProperties: null,
                                  body: body);

            // Gửi thông báo tới SignalR Hub
            var signalRTaskCompletionSource = new TaskCompletionSource<string>();
            //string respone = "";
            //_hubConnection.On<string>("ReceiveMessage", (receivedMessage) =>
            //{
            //    respone = receivedMessage;
            //    if (receivedMessage.Contains(product.Name))
            //    {

            //        signalRTaskCompletionSource.SetResult(receivedMessage);
            //    }
            //});
            //await signalRTaskCompletionSource.Task;
            return Ok(new { status = signalRTaskCompletionSource.Task.Result });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            

            return Ok("Files uploaded successfully.");
        }
    }
}
