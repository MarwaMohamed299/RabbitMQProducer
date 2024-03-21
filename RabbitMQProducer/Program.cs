
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Microsoft.EntityFrameworkCore;
namespace RabbitMQProducer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("orders");
           
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

               // Console.WriteLine(message);

                Console.WriteLine("Message received: " + message);                
            };
            channel.BasicConsume(queue: "orders", autoAck: true, consumer: consumer);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection")));

            builder.Services.AddScoped<IMessageProducer, MessageProducer>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
