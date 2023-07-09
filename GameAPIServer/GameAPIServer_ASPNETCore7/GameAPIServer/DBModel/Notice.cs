namespace GameAPIServer.DBModel;

public class Notice
{
    public static String LoadNotice()
    {
        string filePaths = "Notice.txt";
        string fileContents = File.ReadAllText(filePaths);

        return fileContents;
    }
}