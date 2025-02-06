using NUnit.Framework;

namespace TT.Storage.Npgsql.Tests;

public class NpgsqlDataProviderTests
{
    private readonly string _connectionString = "Server=localhost;Port=11100;Database=postgres;Username=postgres;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";

    [Test]
    public void DataProviderCreateInstaceTest() 
    {
        var dataprovider = new NpgsqlDataProvider(_connectionString);
    }

    [Test]
    public void IsDatabaseExistsTest() 
    {
        var dataprovider = new NpgsqlDataProvider(_connectionString);
        var result = dataprovider.IsDatabaseExists("postgres");
        Assert.IsTrue(result);
        result = dataprovider.IsDatabaseExists("fdfqwr");
        Assert.IsFalse(result);
    }

    [Test]
    public void GetDatabaseVersionTest() 
    {
        var connectoionString = "Server=localhost;Port=11100;Database=tt_test_base;Username=tt_test_role;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";
        var provider = new NpgsqlDataProvider(connectoionString);
        var version = provider.GetDatabaseVersion();
    }
}
