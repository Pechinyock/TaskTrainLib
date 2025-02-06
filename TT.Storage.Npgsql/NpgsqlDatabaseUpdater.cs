using Npgsql;
using System.Text;

namespace TT.Storage.Npgsql;

public sealed class NpgsqlDatabaseUpdater : ISQLDatabaseUpdater
{
    public readonly string _pathToMigrationFolder;

    private string MigrationFolderName => "Migrations";
    private string MigrationUpFolderName => "Up";
    private string MigrationDownFolderName => "Down";
    private string InitializeDatabaseFolderName => "Initialize";
    private string WorkingDbName;

    private readonly NpgsqlDataProvider _dataProvider;

    public NpgsqlDatabaseUpdater(string connectionString, string pathToMigrationFolder)
    {
        if (String.IsNullOrEmpty(pathToMigrationFolder))
            throw new ArgumentNullException(nameof(pathToMigrationFolder));

        _pathToMigrationFolder = pathToMigrationFolder;
        _dataProvider = new NpgsqlDataProvider(connectionString);
        NpgsqlConnectionParameters.Parse(connectionString, out var connectionParams);
        WorkingDbName = connectionParams.Database;
    }

    public bool IsUpdatesAreAvailable()
    {
        if (_dataProvider.GetDatabaseVersion() == 0)
            return true;

        return false;
    }

    public bool IsDatabaseInitialized(string dbName)
    {
        return _dataProvider.IsDatabaseExists(dbName);
    }

    public uint GetAvailabeUpdatesCount()
    {
        if (_dataProvider.GetDatabaseVersion() == 0)
            return 1;

        return 0;
    }

    public IEnumerable<string> GetMigrationsUp()
    {
        var filesPath = Path.Combine(_pathToMigrationFolder
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
        var filesPath = Path.Combine(_pathToMigrationFolder
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
        var filesPath = Path.Combine(_pathToMigrationFolder
            , MigrationFolderName
            , InitializeDatabaseFolderName
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
        var currentVersion = _dataProvider.GetDatabaseVersion();
        if (currentVersion <= 0)
            return;

        var downMigrations = GetMigrationsDown().ToArray();
        if (downMigrations.Length == 0)
            throw new InvalidOperationException();

        Array.Sort(downMigrations);

        var previousVersion = --currentVersion;
        var targetMigration = downMigrations[previousVersion];
        var migrationPath = BuildFullPathToMigrations(MigrationDownFolderName, targetMigration);

        var query = File.ReadAllText(migrationPath);
        if (String.IsNullOrEmpty(query))
            throw new InvalidOperationException();

        _dataProvider.ExecuteNonQuery(query);
        if (previousVersion > 0)
            _dataProvider.ExecuteNonQuery($"update database_metainfo set database_version = {previousVersion} where sigle_row = true");
    }

    public void StepForward()
    {
        var currentVersion = _dataProvider.GetDatabaseVersion();

        var upMigrations = GetMigrationsUp().ToArray();
        if (upMigrations.Length == 0)
            throw new InvalidOperationException();

        if (upMigrations.Length < currentVersion)
            return;

        Array.Sort(upMigrations);

        var targetMigration = upMigrations[currentVersion];

        var migrationPath = BuildFullPathToMigrations(MigrationUpFolderName, targetMigration);
        var query = File.ReadAllText(migrationPath);
        if (String.IsNullOrEmpty(query))
            throw new InvalidOperationException();

        _dataProvider.ExecuteNonQuery(query);
        var newVersion = ++currentVersion;
        _dataProvider.ExecuteNonQuery($"update database_metainfo set database_version = {newVersion} where sigle_row = true");

    }

    public void Initialize()
    {
        var migrations = GetInitializeDatabaseRecipe().ToArray();

        Array.Sort(migrations);

        foreach (var migrationFileName in migrations)
        {
            var migrationPath = BuildFullPathToMigrations(InitializeDatabaseFolderName, migrationFileName);
            var query = File.ReadAllText(migrationPath);
            if (String.IsNullOrEmpty(query))
                throw new InvalidOperationException();

            _dataProvider.ExecuteNonQuery(query);
        }
    }

    public void Migrate(uint version)
    {
    }

    private string BuildFullPathToMigrations(string migrationDirection, string fileName)
    {
        var result = Path.Combine(_pathToMigrationFolder
            , MigrationFolderName
            , migrationDirection
            , fileName
        );
        return result;
    }

    private uint ExtractMigrationNumber(string migrationFileName)
    {
        if (String.IsNullOrEmpty(migrationFileName))
            throw new ArgumentNullException(nameof(migrationFileName));

        const char delimeter = '-';
        var stringBulder = new StringBuilder();
        foreach (var c in migrationFileName)
        {
            if (c == delimeter)
                break;

            stringBulder.Append(c);
        }
        var extractedNumStr = stringBulder.ToString();
        if (!uint.TryParse(extractedNumStr, out var result))
            throw new InvalidOperationException($"file: {migrationFileName} has wrong file name format");

        return result;
    }
}
