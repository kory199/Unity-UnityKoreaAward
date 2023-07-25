using System.Collections.Generic;


[System.Serializable]
public class PlayerStatus
{
    public int Id;
    public int Level;
    public int HP;
    public float MovementSpeed;
    public int AttackPower;
    public float AttackSpeed;
    public int XPRequiredForLevelUp;
    public static Dictionary<int, PlayerStatus> table = new Dictionary<int, PlayerStatus> ();   
}