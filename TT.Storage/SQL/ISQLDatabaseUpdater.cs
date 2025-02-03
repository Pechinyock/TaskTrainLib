namespace TT.Storage;

public interface ISQLDatabaseUpdater
{
    IEnumerable<string> GetMigrationsUpList();
    IEnumerable<string> GetMigrationsDownList();
    void Migrate(uint version);
    void StepForward();
    void StepBack();
}