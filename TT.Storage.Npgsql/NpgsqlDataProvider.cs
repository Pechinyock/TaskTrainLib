using Npgsql;

namespace TT.Storage.Npgsql;

public sealed class NpgsqlDataProvider : ISQLDataProvider
{
    private readonly string _connectionString;
    private readonly string _workingDatabaseName;

    public NpgsqlDataProvider(string connectionString)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        var strBuiler = new NpgsqlConnectionStringBuilder(_connectionString);
        _connectionString = connectionString;
        _workingDatabaseName = strBuiler.Database;
    }

    public bool IsDatabaseExists(string dbName)
    {
        if (String.IsNullOrEmpty(dbName))
            return false;

        var cmdStr = $"select exists(select datname from pg_catalog.pg_database where lower(datname) = lower('{dbName}'));";

        bool result = false;
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = cmdStr;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = (bool)reader["exists"];
                }
            }
        }
        return result;
    }

    public void ExecuteNonQuery(string sqlText)
    {
        if (String.IsNullOrEmpty(sqlText))
            return;
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlText;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (NpgsqlException ex) 
        {
            throw ex;
        }

    }

    public string WorkingDatabaseName() => _workingDatabaseName;
}
