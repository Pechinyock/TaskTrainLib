namespace TT.Storage.Npgsql;

public class NpgsqlConnectionParameters
{
    public Dictionary<string, string> AllParameters { get; private set; } = new Dictionary<string, string>();
    public string Server => AllParameters[nameof(Server)];
    public string Port => AllParameters[nameof(Port)];
    public string Database => AllParameters[nameof(Database)];

    public static void Parse(string connectionString, out NpgsqlConnectionParameters result) 
    {
        if(String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        result = new NpgsqlConnectionParameters();

        const char kvpDelimeter = ';';
        const char kvDelimeter = '=';

        var splitKVP = connectionString.Split(kvpDelimeter);
        foreach (var kv in splitKVP) 
        {
            var values = kv.Split(kvDelimeter);
            if (values.Length < 2)
                continue;

            result.AllParameters.Add(values[0], values[1]);
        }
    }
}
