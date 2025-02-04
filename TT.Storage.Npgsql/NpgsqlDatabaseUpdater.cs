using Npgsql;

namespace TT.Storage.Npgsql;

public sealed class NpgsqlDatabaseUpdater : ISQLDatabaseUpdater
{
    private string MigrationFolderName => "Migrations";
    private string MigrationUpFolderName => "Up";
    private string MigrationDownFolderName => "Down";

    private readonly NpgsqlDataProvider _dataProvider;

    public NpgsqlDatabaseUpdater(NpgsqlDataProvider dataProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProvider, nameof(dataProvider));
        _dataProvider = dataProvider;
    }

    public IEnumerable<string> GetMigrationsUpList()
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

    public IEnumerable<string> GetMigrationsDownList()
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
        uint initializeQueriesCount = 2;
        var migrations = GetMigrationsUpList().ToArray();
        var migrationsCount = migrations.Length;
        if (initializeQueriesCount < migrationsCount)
            throw new InvalidOperationException();

        Array.Sort(migrations);

        foreach (var migrationFileName in migrations)
        {
            var migrationPath = BuildFullPathToMigrations(MigrationUpFolderName, migrationFileName);
            var query = File.ReadAllText(migrationPath);
            if (String.IsNullOrEmpty(query))
                throw new InvalidOperationException();

            _dataProvider.ExecuteNonQuery(query);
        }
    }
}
