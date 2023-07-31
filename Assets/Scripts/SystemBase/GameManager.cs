using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoSingleton<GameManager>
{


    private void Start()
    {
        foreach (var data in SkillData.table.Values)
        {
            Debug.Log(data.Id);
            Debug.Log(data.Description);
            Debug.Log(data.Name);
            Debug.Log(data.PlayerSkiilsType);
            Debug.Log(data.UnlockLevel);
        }
    }
}
