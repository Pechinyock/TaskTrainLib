namespace TT.Core;

public class ExchangeInfo 
{
    public string Name;
    public ExchangeTypeEnum ExchangeType;
    public string RoutingKey;
}

public interface IMessagePublisher
{
    public Task Send(string exchange, ExchangeTypeEnum type, string routingKey, ReadOnlyMemory<byte> body);

    public Task Send(ExchangeInfo envelope, ReadOnlyMemory<byte> body);
}
