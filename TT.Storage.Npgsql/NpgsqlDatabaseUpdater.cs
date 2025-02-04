using Npgsql;

namespace TT.Storage.Npgsql;

public sealed class NpgsqlDatabaseUpdater : ISQLDatabaseUpdater
                                          , IDisposable
{
    private string MigrationFolderName => "Migrations";
    private string MigrationUpFolderName => "Up";
    private string MigrationDownFolderName => "Down";
    private string MigrationInitializeBaseName => "Initialize";

    private readonly NpgsqlDataProvider _dataProvider;

    public NpgsqlDatabaseUpdater(string connectionString)
    {
        /* parse connection string if it's not connecting to 'postgres' database
         * we throws an error - invalid op.
         */
        _dataProvider = new NpgsqlDataProvider(connectionString);
    }

    public void CheckForUpdates(string databaseName)
    {
        if(String.IsNullOrEmpty(databaseName))
            throw new ArgumentNullException(nameof(databaseName));

        if (!_dataProvider.IsDatabaseExists(databaseName))
            Initialize();
    }

    public IEnumerable<string> GetMigrationsUp()
    {
        var filesPath = Path.Combine(AppContext.BaseDirectory
            , MigrationFolderName
            , MigrationUpFolderName
        );

        var queriesPaths = Directory.GetFiles(filesPath);
        var result = new string[queriesPaths.Length];
        for (int i = 0; i < queriesPaths.Length; ++i)
        {
            result[i] = Path.GetFileName(queriesPaths[i]);
        }
        return result;
    }

    public IEnumerable<string> GetMigrationsDown()
    {
        var filesPath = Path.Combine(AppContext.BaseDirectory
            , MigrationFolderName
            , MigrationDownFolderName
        );

        var queriesPaths = Directory.GetFiles(filesPath);
        var result = new string[queriesPaths.Length];
        for (int i = 0; i < queriesPaths.Length; ++i)
        {
            result[i] = Path.GetFileName(queriesPaths[i]);
        }
        return result;
    }

    public IEnumerable<string> GetInitializeDatabaseRecipe() 
    {
        var filesPath = Path.Combine(AppContext.BaseDirectory
            , MigrationFolderName
            , MigrationInitializeBaseName
        );

        var queriesPaths = Directory.GetFiles(filesPath);
        var result = new string[queriesPaths.Length];
        for (int i = 0; i < queriesPaths.Length; ++i)
        {
            result[i] = Path.GetFileName(queriesPaths[i]);
        }
        return result;
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

    public void Dispose() => _dataProvider.Dispose();

    private string BuildFullPathToMigrations(string migrationDirection, string fileName)
    {
        var result = Path.Combine(AppContext.BaseDirectory
            , MigrationFolderName
            , migrationDirection
            , fileName
        );
        return result;
    }

    private void Initialize()
    {
        var migrations = GetInitializeDatabaseRecipe().ToArray();

        Array.Sort(migrations);

        foreach (var migrationFileName in migrations)
        {
            var migrationPath = BuildFullPathToMigrations(MigrationInitializeBaseName, migrationFileName);
            var query = File.ReadAllText(migrationPath);
            if (String.IsNullOrEmpty(query))
                throw new InvalidOperationException();

            _dataProvider.ExecuteNonQuery(query);
        }
    }
}
