using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TT.Core;

#region options class
public class RabbitMQPublisherOptions 
{
    public string Host { get; set; }
}
#endregion

public class RabbitMQPublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMQPublisher(IOptions<RabbitMQPublisherOptions> options)
    {
        var host = options.Value.Host;
        var factory = new ConnectionFactory { HostName = host };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task Send(string exchange, ExchangeTypeEnum type, string routingKey, ReadOnlyMemory<byte> body)
    {
        await _channel.ExchangeDeclareAsync(exchange, RabbitMQUtils.GetExchangeType(type));
        await _channel.BasicPublishAsync(exchange, routingKey, body);
    }

    public void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
    }
}
