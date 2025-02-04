﻿namespace TT.Storage;

public interface ISQLDatabaseUpdater
{
    IEnumerable<string> GetMigrationsUp();
    IEnumerable<string> GetMigrationsDown();
    IEnumerable<string> GetInitializeDatabaseRecipe();
    void Migrate(uint version);
    void StepForward();
    void StepBack();
    void CheckForUpdates(string databaseName);
}