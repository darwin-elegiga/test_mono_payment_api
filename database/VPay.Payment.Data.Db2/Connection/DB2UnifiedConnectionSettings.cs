namespace VPay.Payment.Data.Db2.Connection;

// ReSharper disable once InconsistentNaming
public class DB2UnifiedConnectionSettings
{
    public bool IsLocal { get; set; } = true;

    public string Dsn { get; set; }

    public string Hostname { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string DefaultLibraries { get; set; }
}
