using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq;
using RabbitMq.Model;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://127.0.0.1:5454");

builder.Configuration.AddJsonFile(
    "appsettings.json", 
    optional: 
    true, 
    reloadOnChange: true);

builder
    .Services
    .AddRabbitMQ(builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQEntity>())
    .AddHostedService(services => new Worker(services, "TestRBQ"));

var app = builder.Build();

using var scope = app.Services.CreateScope();
using var channel = scope.ServiceProvider.GetRequiredService<IConnection>().CreateModel();


var msg = "Hello Stefi!!!";

channel.QueueDeclare(
    queue: "TestRBQ",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

channel.BasicPublish(
    exchange: "", 
    routingKey: "TestRBQ", 
    basicProperties: null, 
    body: Encoding.UTF8.GetBytes(msg));

Console.WriteLine($" [x] Sent '{msg}' to '{"TestRBQ"}'");

app.Run();