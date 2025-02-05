using NUnit.Framework;

namespace TT.Storage.Npgsql.Tests;

public class NpgsqlDatabaseUpdaterTests
{
    private readonly string _connectionString = "Server=localhost;Port=11100;Database=postgres;Username=postgres;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";

    [Test]
    public void TestDatabaseAccessableTest()
    {
        var pgUpdater = new NpgsqlDatabaseUpdater(_connectionString);
        Assert.IsNotNull(pgUpdater);
    }

    [Test]
    public void MigrationListsAvailableTest()
    {
        var pgUpdater = new NpgsqlDatabaseUpdater(_connectionString);

        var listUp = pgUpdater.GetMigrationsUp();
        Assert.IsNotNull(listUp);
        var upElementsCount = listUp.Count();
        Assert.Less(0, upElementsCount);

        var listDown = pgUpdater.GetMigrationsDown();
        Assert.IsNotNull(listDown);

        var downElemetsCount = listDown.Count();
        Assert.Less(0, downElemetsCount);
    }

    [Test]
    public void InitializeDatabaseTest() 
    {
        var pgUpdater = new NpgsqlDatabaseUpdater(_connectionString);
        pgUpdater.Initialize();
    }

    [Test]
    public void StepForwardTest() 
    {
        var connectoionString = "Server=localhost;Port=11100;Database=tt_test_base;Username=tt_test_role;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";
        var pgUpdater = new NpgsqlDatabaseUpdater(connectoionString);
        pgUpdater.StepForward();
    }
}
