namespace TT.Core;

public static partial class PublishRegestry
{
    public static readonly ExchangeInfo Logger = new ExchangeInfo()
    {
        Name = "Log",
        ExchangeType = ExchangeTypeEnum.Fanout,
        RoutingKey = String.Empty
    };
}