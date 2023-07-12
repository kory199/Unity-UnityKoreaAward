using System.Collections.Generic;


[System.Serializable]
public class StringData
{
    public int ID;
    public string Descript;
    public string Kor;
    public static Dictionary<int, StringData> table = new Dictionary<int, StringData> ();   
}