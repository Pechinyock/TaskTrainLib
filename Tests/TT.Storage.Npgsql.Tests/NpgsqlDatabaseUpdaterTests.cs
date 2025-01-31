using NUnit.Framework;

namespace TT.Storage.Npgsql.Tests;

public class NpgsqlDatabaseUpdaterTests
{
    private readonly string _connectionString = "Server=localhost;Port=11100;Database=postgres;Username=postgres;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";

    [Test]
    public void TestDatabaseAccessable() 
    {
        var pgUpdater = new NpgsqlDatabaseUpdater(_connectionString);
    }
}
