using Npgsql;

namespace TT.Storage.Npgsql;

public sealed class NpgsqlDataProvider : ISQLDataProvider
{
    private readonly string _connectionString;

    public NpgsqlDataProvider(string connectionString)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
    }

    public bool IsDatabaseExists(string dbName)
    {
        if (String.IsNullOrEmpty(dbName))
            return false;

        bool result = false;
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = $"select exists(select datname from pg_catalog.pg_database where lower(datname) = lower('{dbName}'));";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = (bool)reader["exists"];
                }
            }
            return result;
        }
    }

    public void ExecuteNonQuery(string sqlText)
    {
        if (String.IsNullOrEmpty(sqlText))
            return;

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

    public int GetDatabaseVersion()
    {
        int result = 0;

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            bool isMetadataTableExists = false;
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = $"select exists (select from information_schema.tables where table_schema = 'public' and table_name = 'database_metainfo');";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    isMetadataTableExists = (bool)reader["exists"];
                }
            }
            if (!isMetadataTableExists)
                return result;

            connection.Close();
        }

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                var columnName = "database_version";
                cmd.CommandText = $"select {columnName} from database_metainfo";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = (int)reader[$"{columnName}"];
                }
            }
            connection.Close();
        }

        return result;
    }
}
