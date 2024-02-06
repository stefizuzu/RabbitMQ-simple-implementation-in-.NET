using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Model;
using RabbitMQ.Client;

public static class ServiceExtensions
{

    /// <summary>
    /// Create and add RabitMQ instance.
    /// </summary>
    public static IServiceCollection AddRabbitMQ(
        this IServiceCollection services,
        RabbitMQEntity connRbqCredentials
        )
        => services
        .AddScoped(_ =>
            new ConnectionFactory
            {
                HostName = connRbqCredentials.hostname,
                Port = connRbqCredentials.port,
                UserName = connRbqCredentials.username,
                Password = connRbqCredentials.password,
                AutomaticRecoveryEnabled = connRbqCredentials.automaticRecoveryEnabled,
            }
            .CreateConnection()
        )
        .AddScoped<Func<IModel>>(
                services =>
                    () =>
                        services
                            .GetRequiredService<IConnection>()
                            .CreateModel()
        )
        .AddTransient(
                services =>
                    services
                        .GetRequiredService<IConnection>()
                        .CreateModel()
        );
        //.AddScoped<IPublisher, Publisher>();
}