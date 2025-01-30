namespace TT.Core;

public interface ITTApp
{
    void Build(string[] args);
    void Run();
}

public interface IStorageProvider 
{
    StroageTypeFlags GetType();

    string GetDatabaseVendorName();

    string GetDefautDatabaseName();
}