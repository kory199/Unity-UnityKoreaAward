using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoTest : MonoBehaviour
{
    PlayerBaseData playerBaseData;

    [SerializeField] TextMeshProUGUI hp;
    [SerializeField] TextMeshProUGUI lv;
    [SerializeField] TextMeshProUGUI exp;

    public void PlayerInfo()
    {
        hp.text = playerBaseData.hp.ToString();
        lv.text = playerBaseData.level.ToString();
        exp.text = playerBaseData.exp.ToString();
    }
}
