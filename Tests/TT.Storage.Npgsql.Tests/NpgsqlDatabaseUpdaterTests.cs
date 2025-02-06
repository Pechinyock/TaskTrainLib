using NUnit.Framework;

namespace TT.Storage.Npgsql.Tests;

public class NpgsqlDatabaseUpdaterTests
{
    private readonly string _connectionString = "Server=localhost;Port=11100;Database=postgres;Username=postgres;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";

    [Test, Order(1)]
    public void TestDatabaseAccessableTest()
    {
        var pgUpdater = new NpgsqlDatabaseUpdater(_connectionString, AppContext.BaseDirectory);
        Assert.IsNotNull(pgUpdater);
    }

    [Test, Order(2)]
    public void MigrationListsAvailableTest()
    {
        var pgUpdater = new NpgsqlDatabaseUpdater(_connectionString, AppContext.BaseDirectory);

        var listUp = pgUpdater.GetMigrationsUp();
        Assert.IsNotNull(listUp);
        var upElementsCount = listUp.Count();
        Assert.Less(0, upElementsCount);

        var listDown = pgUpdater.GetMigrationsDown();
        Assert.IsNotNull(listDown);

        var downElemetsCount = listDown.Count();
        Assert.Less(0, downElemetsCount);
    }

    [Test, Order(3)]
    public void InitializeDatabaseTest() 
    {
        var pgUpdater = new NpgsqlDatabaseUpdater(_connectionString, AppContext.BaseDirectory);
        pgUpdater.Initialize();
    }

    [Test, Order(4)]
    public void StepForwardTest() 
    {
        var connectoionString = "Server=localhost;Port=11100;Database=tt_test_base;Username=tt_test_role;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";
        var pgUpdater = new NpgsqlDatabaseUpdater(connectoionString, AppContext.BaseDirectory);
        pgUpdater.StepForward();
    }

    [Test, Order(5)]
    public void StepBackTest()
    {
        var connectoionString = "Server=localhost;Port=11100;Database=tt_test_base;Username=tt_test_role;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";
        var pgUpdater = new NpgsqlDatabaseUpdater(connectoionString, AppContext.BaseDirectory);
        pgUpdater.StepBack();
    }
}
