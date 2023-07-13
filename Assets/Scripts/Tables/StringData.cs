using System.Collections.Generic;


[System.Serializable]
public class StringData
{
    public int Id;
    public string Description;
    public string Kor;
    public string Eng;
    public static Dictionary<int, StringData> table = new Dictionary<int, StringData> ();   
}