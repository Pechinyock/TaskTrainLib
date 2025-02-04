using Npgsql;

namespace TT.Storage.Npgsql;

public sealed class NpgsqlDataProvider : ISQLDataProvider
                                       , IDisposable
{
    private readonly string _connectionString;
    private readonly string _workingDatabaseName;
    private readonly NpgsqlConnection _connection;

    public NpgsqlDataProvider(string connectionString)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();

        /* parse connection string and fill _workingDatabaseName */
    }

    public bool IsDatabaseExists(string dbName)
    {
        if (String.IsNullOrEmpty(dbName))
            return false;

        var cmdStr = $"select exists(select datname from pg_catalog.pg_database where lower(datname) = lower('{dbName}'));";

        bool result = false;

        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = _connection;
            cmd.CommandText = cmdStr;
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = (bool)reader["exists"];
            }
        }
        return result;
    }

    public void ExecuteNonQuery(string sqlText)
    {
        if (String.IsNullOrEmpty(sqlText))
            return;

        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = _connection;
            cmd.CommandText = sqlText;
            cmd.ExecuteNonQuery();
        }
    }

    public string WorkingDatabaseName() => _workingDatabaseName;

    public void Dispose() => _connection.Dispose();
}
