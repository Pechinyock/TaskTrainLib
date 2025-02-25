using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Core;

internal static class RabbitMQUtils
{
    public static string GetExchangeType(ExchangeTypeEnum value)
    {
        switch (value)
        {
            case ExchangeTypeEnum.Fanout:
                return ExchangeType.Fanout;
            case ExchangeTypeEnum.Direct:
                return ExchangeType.Direct;
            default:
                throw new InvalidOperationException("Unknown value");
        }
    }
}
