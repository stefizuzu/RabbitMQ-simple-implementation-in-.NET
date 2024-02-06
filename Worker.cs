using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq
{
    public class Worker : BackgroundService
    {
        public IServiceProvider _services { get; }
        public string _queue { get; } = "TestRBQ";

        public Worker(IServiceProvider services, string queue) 
        {
            _services = services;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();

            var channel = 
                scope
                .ServiceProvider
                .GetRequiredService<IConnection>()
                .CreateModel();

            channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine($" [x] Cancellation requested. Stopping processing for '{_queue}'");
                    return;
                }

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received '{message}' from '{_queue}'");
            };

            channel.BasicConsume(queue: _queue, autoAck: true, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
