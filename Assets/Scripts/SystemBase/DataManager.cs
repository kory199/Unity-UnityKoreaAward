using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager :MonoSingleton<DataManager>
{
    public MonsterData MonsterData = null;

    protected override void Awake()
    {
        base.Awake();

        if(MonsterData = null)
        {
            MonsterData = new();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
