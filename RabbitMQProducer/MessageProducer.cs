using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQProducer
{
    public class MessageProducer : IMessageProducer
    {
        public void SendMessages<T>(T message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
          //  channel.QueueDeclare("orders");

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "orders", body: body);

        }
        
    }
}
