namespace TT.Core;

[Flags]
public enum StroageTypeFlags
{
    None =          0,
    Local =         1 << 0,
    Remote =        1 << 1,
    SqlDatabase =   1 << 2,
    NoSqlDatabase = 1 << 3,
}

public enum AccessLevelEnum
{
    Any   = 0,
    Admin = 1,
    Owner = 2
}