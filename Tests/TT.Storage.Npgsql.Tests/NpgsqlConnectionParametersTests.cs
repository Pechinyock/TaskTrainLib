using NUnit.Framework;

namespace TT.Storage.Npgsql.Tests;

public class NpgsqlConnectionParametersTests
{
    [Test]
    public void ParseTest() 
    {
        var connectionString = "Server=localhost;Port=11100;Database=postgres;Username=postgres;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";
        NpgsqlConnectionParameters.Parse(connectionString, out var parsed);
        Assert.IsNotNull(parsed);
        Assert.AreEqual("localhost", parsed.Server);
        Assert.AreEqual("11100", parsed.Port);
        Assert.AreEqual("postgres", parsed.Database);
    }
}
