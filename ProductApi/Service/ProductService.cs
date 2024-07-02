using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using RabbitMQ.Client;

namespace ProductApi.Service
{
    public class ProductService
    {
        public void SaveFiles(List<IFormFile> files, IModel _channel)
        {
            foreach (IFormFile file in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    var message = JsonSerializer.Serialize(new { FileName = file.FileName, FileContent = fileBytes });
                    var body = Encoding.UTF8.GetBytes(message);

                    _channel.BasicPublish(exchange: "",
                                          routingKey: "file",
                                          basicProperties: null,
                                          body: body);
                }
            }
        }
    }
}
