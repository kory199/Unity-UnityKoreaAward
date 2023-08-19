using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager :MonoSingleton<DataManager>
{
    #region Unity lifecycle
    private void Awake()
    {
         
    }

    private void Start()
    {
        
    }
    #endregion

    private void GameDataSetting()
    {
        foreach (var gamedata in collection)
        {

        }
    }

    // 추후 서버에 저장될 데이터 
    private void GameInfoData()
    {

    }
}
