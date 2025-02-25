namespace TT.Core;

public interface IMessagePublisher
{
    public Task Send(string exchange, ExchangeTypeEnum type, string routingKey, ReadOnlyMemory<byte> body);
}
