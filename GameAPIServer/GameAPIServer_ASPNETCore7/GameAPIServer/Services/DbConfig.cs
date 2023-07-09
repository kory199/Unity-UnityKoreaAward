namespace GameAPIServer.Services;

public class DbConfig
{
    //private String VersionDb;

    //public String GetVersionDb() => VersionDb;
    public String VerDb { get; set; }
    public String MasterDataDb { get; set; }
    public String AccountDb { get; set; }
    public String GameDb { get; set; }
}