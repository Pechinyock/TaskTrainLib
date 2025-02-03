using Npgsql;
using TT.Storage;

namespace TT.Storage.Npgsql;

public class NpgsqlDatabaseUpdater : ISQLDatabaseUpdater
{
    private readonly string _connectionString;
    private readonly string _defaultPgDatabaseName = "postgres";

    public NpgsqlDatabaseUpdater(string connectionString)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
    }

    public IEnumerable<string> GetMigrationsUpList()
    {
        var filesPath = Path.Combine(AppContext.BaseDirectory, "Migrations", "Up");
        var queriesPaths = Directory.GetFiles(filesPath);
        return queriesPaths;
    }

    public IEnumerable<string> GetMigrationsDownList()
    {
        var filesPath = Path.Combine(AppContext.BaseDirectory, "Migrations", "Down");
        var queriesPaths = Directory.GetFiles(filesPath);
        return queriesPaths;
    }

    public void StepBack()
    {
        throw new NotImplementedException();
    }

    public void StepForward()
    {
        throw new NotImplementedException();
    }

    public void Migrate(uint version)
    {
        throw new NotImplementedException();
    }
}
