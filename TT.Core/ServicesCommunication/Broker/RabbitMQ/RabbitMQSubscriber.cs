using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TT.Core;

#region options class
public class RabbitMQSubscriberOptions
{
    public string Host { get; set; }
    public string ExchangeName { get; set; }
    public ExchangeTypeEnum ExchangeType { get; set; }
    public string RoutingKey { get; set; }
    public string QueueName { get; set; }
    public Action<byte[]> OnMessageRecived { get; set; }
}

#endregion

public class RabbitMQSubscriber : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly IAsyncBasicConsumer _consumer;

    private readonly string _queueName;

    private Action<byte[]> _onMessageRecived;

    public RabbitMQSubscriber(IOptions<RabbitMQSubscriberOptions> options)
    {
        var host = options.Value.Host;
        var exchangeName = options.Value.ExchangeName;
        var exchangeType = RabbitMQUtils.GetExchangeType(options.Value.ExchangeType);
        var queueName = options.Value.QueueName ?? String.Empty;
        var routingKey = options.Value.RoutingKey ?? String.Empty;

        var factory = new ConnectionFactory { HostName = host };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.ExchangeDeclareAsync(exchangeName, exchangeType).Wait();

        var queue = _channel.QueueDeclareAsync(queueName).GetAwaiter().GetResult();
        _queueName = queue.QueueName;
        _channel.QueueBindAsync(_queueName, exchangeName, routingKey);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += Consumer_ReceivedAsync;

        _consumer = consumer;
        _onMessageRecived = options.Value.OnMessageRecived;
    }

    private Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs message)
    {
        var body = message.Body.ToArray();
        if (body.Any())
        {
            _onMessageRecived(body);
        }
        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _channel.BasicConsumeAsync(_queueName, true, _consumer, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();

        base.Dispose();
    }
}
