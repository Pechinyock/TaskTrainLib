namespace TT.Core;

public class AuthOptions
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Key { get; set; }
    public uint Lifetime { get; set; }
}
