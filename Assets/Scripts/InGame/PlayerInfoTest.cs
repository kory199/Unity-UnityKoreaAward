using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoTest : MonoBehaviour
{
    public PlayerBaseData playerBaseData;

    [SerializeField] TextMeshProUGUI hp;
    [SerializeField] TextMeshProUGUI lv;
    [SerializeField] TextMeshProUGUI exp;

    private void Start()
    {
        playerBaseData = Resources.Load<PlayerBaseData>("PlayerData");
    }

    private void Update()
    {
        PlayerInfo();
    }

    public void PlayerInfo()
    {
        hp.text = playerBaseData.hp.ToString();
        lv.text = playerBaseData.level.ToString();
        exp.text = playerBaseData.exp.ToString();
    }
}
