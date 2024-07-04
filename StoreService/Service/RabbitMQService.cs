using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StoreService.EntityModels;
using StoreService.Interface;
using StoreService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IModel = RabbitMQ.Client.IModel;

namespace StoreService.Service
{
    public class RabbitMQService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "orderQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public void OrderListener()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
                {

                    var body = ea.Body.ToArray();
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                        var order = JsonConvert.DeserializeObject<OrderRequest>(Encoding.UTF8.GetString(body));

                        if (order?.Products == null || order.Products.Any(p => p.price == null))
                        {
                            Console.WriteLine("Order contains products with null price.");
                            return;
                        }

                        bool status = await orderService.ProcessOrder(order);
                        UpdateStatus(status, order.Id);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON deserialization error: {ex.Message}");
                    }
            };

            _channel.BasicConsume(queue: "orderQueue",
                                 autoAck: true,
                                 consumer: consumer);
            }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

        public async void UpdateStatus(bool  status, Guid orderId)
        {
            OrderStatus orderStatus = new OrderStatus{
                OrderId = orderId,
                Status = status
            };
            _channel.QueueDeclare(queue: "statusQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderStatus));
            _channel.BasicPublish(exchange: "", routingKey: "statusQueue", basicProperties: null, body: body);
        }
    }
}
