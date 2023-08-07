using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager :MonoSingleton<DataManager>
{
    public MonsterData MonsterData = null;

    private void Awake()
    {

        if(MonsterData = null)
        {
            MonsterData = new();
        }
    }

}
