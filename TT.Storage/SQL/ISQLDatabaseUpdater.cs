namespace TT.Storage;

public interface ISQLDatabaseUpdater
{
    IEnumerable<string> GetMigrationList();
    string GetWorkingDatabaseName();
    uint GetCurrentVersion();
    void Update(uint version);
    void Downgrade(uint version);
    void StepForward();
    void StepBack();
}