using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Model;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Consumer
{
    public class ProductConsumer : IConsumer<Product>
    {
        public async Task Consume(ConsumeContext<Product> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"OrderCreated message: {jsonMessage}");
        }
    }
}
