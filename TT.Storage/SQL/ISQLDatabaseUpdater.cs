namespace TT.Storage;

public interface ISQLDatabaseUpdater
{
    IEnumerable<string> GetMigrationsUp();
    IEnumerable<string> GetMigrationsDown();
    IEnumerable<string> GetInitializeDatabaseRecipe();
    void StepForward();
    void StepBack();
    bool IsDatabaseInitialized(string dbName);
    void Initialize();
}