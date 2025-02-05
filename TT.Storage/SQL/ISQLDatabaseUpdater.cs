namespace TT.Storage;

public interface ISQLDatabaseUpdater
{
    IEnumerable<string> GetMigrationsUp();
    IEnumerable<string> GetMigrationsDown();
    IEnumerable<string> GetInitializeDatabaseRecipe();
    void Migrate(uint version);
    void StepForward();
    void StepBack();
    bool IsUpdatesAreAvailable();
    uint GetAvailabeUpdatesCount();
    void Initialize();
}