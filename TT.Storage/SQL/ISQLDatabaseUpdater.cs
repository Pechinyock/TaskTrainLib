namespace TT.Storage;

public interface ISQLDatabaseUpdater
{
    void Initialize();
    IEnumerable<string> GetMigrationsUpList();
    IEnumerable<string> GetMigrationsDownList();
    void Migrate(uint version);
    void StepForward();
    void StepBack();
}