using Npgsql;
using TT.Stroage;

namespace TT.Storage.Npgsql;

public class NpgsqlDatabaseUpdater : ISQLDatabaseUpdater
{
    private readonly string _connectionString;

    public NpgsqlDatabaseUpdater(string connectionString)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
    }

    public void Downgrade(uint version)
    {
        throw new NotImplementedException();
    }

    public uint GetCurrentVersion()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetMigrationList()
    {
        throw new NotImplementedException();
    }

    public string GetWorkingDatabaseName()
    {
        throw new NotImplementedException();
    }

    public void StepBack()
    {
        throw new NotImplementedException();
    }

    public void StepForward()
    {
        throw new NotImplementedException();
    }

    public void Update(uint version)
    {
        throw new NotImplementedException();
    }
}
