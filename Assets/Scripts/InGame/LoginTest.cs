using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginTest : MonoBehaviour
{
    [Header("Account")]
    [SerializeField] TextMeshPro input_ID = null;
    [SerializeField] TextMeshPro input_EXP = null;
    [SerializeField] TextMeshPro input_HP = null;
    [SerializeField] Button PlayerStatusClosedButton = null;
    [SerializeField] Button RankingButtonClosedButton = null;
    [SerializeField] GameObject PlayerInfoPanel = null;
    [SerializeField] GameObject RankingPanel = null;

    private void Start()
    {
        PlayerInfoPanel.SetActive(false);
        RankingPanel.SetActive(false);
    }

    public void PlayerInfoCloseButton()
    {
        PlayerInfoPanel.SetActive(false);
    }

    public void PlayerInfoButton()
    {
        PlayerInfoPanel.SetActive(true);
    }

    public void RankingCloseButton()
    {
        RankingPanel.SetActive(false);
    }
    public void RankingInfoButton()
    {
        RankingPanel.SetActive(true);
    }
}