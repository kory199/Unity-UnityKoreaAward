using System.Collections;
using System.Collections.Generic;
using APIModels;
using Unity.VisualScripting;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    async void Start()
    {
        bool result = await APIManager.Instance.GetMasterDataAPI();

        Debug.Log($"result : {result}");

        PlayerStatus_res[] playerStatus = APIManager.Instance.GetValueByKey<PlayerStatus_res[]>(MasterDataDicKey.PlayerStatus.ToString());
        Debug.Log($"hp : {playerStatus[0].hp}");
        Debug.Log($"attack_power : {playerStatus[0].attack_power}");
        Debug.Log($"xp_requiredfor_levelup : {playerStatus[0].xp_requiredfor_levelup}");
        Debug.Log($"projectile_speed : {playerStatus[0].projectile_speed}");
        Debug.Log($"rate_of_fire : {playerStatus[0].rate_of_fire}");
    }
}
// 한글테스트 한글 테스트