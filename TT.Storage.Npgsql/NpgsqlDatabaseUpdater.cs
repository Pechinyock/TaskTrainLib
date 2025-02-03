using Npgsql;
using TT.Storage;

namespace TT.Storage.Npgsql;

public sealed class NpgsqlDatabaseUpdater : ISQLDatabaseUpdater
{
    private string MigrationFolderName { get { return "Migrations"; } }
    private string MigrationUpFolderName { get { return "Up"; } }
    private string MigrationDownFolderName { get { return "Down"; } }

    private readonly NpgsqlDataProvider _dataProvider;

    public NpgsqlDatabaseUpdater(NpgsqlDataProvider dataProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProvider, nameof(dataProvider));
        _dataProvider = dataProvider;
    }

    public void Initialize()
    {
        uint initializeQueriesCount = 2;
        var migrationsPaths = GetMigrationsUpList();
        if (initializeQueriesCount < migrationsPaths.Count())
            throw new InvalidOperationException();

        /* sort migrations */

        var filePathFileName = new Dictionary<string, string>();

        foreach (var migrationPath in migrationsPaths) 
        {
            var query = File.ReadAllText(migrationPath);
            if(String.IsNullOrEmpty(query))
                throw new InvalidOperationException();

        }
    }

    public IEnumerable<string> GetMigrationsUpList()
    {
        var filesPath = Path.Combine(AppContext.BaseDirectory
            , MigrationFolderName
            , MigrationUpFolderName
        );

        var queriesPaths = Directory.GetFiles(filesPath);
        var result = new string[queriesPaths.Length];
        for(int i = 0; i < queriesPaths.Length; ++i)
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
}
