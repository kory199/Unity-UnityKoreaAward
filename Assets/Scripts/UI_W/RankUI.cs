using System.Collections;
using System.Collections.Generic;
using APIModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankUI : MonoBehaviour
{
    [SerializeField] Button r_goBackBut = null;
    [SerializeField] TextMeshProUGUI[] rankTopThreeName = null;
    [SerializeField] TextMeshProUGUI[] rankTopTen = null;
    [SerializeField] TextMeshProUGUI userRank = null;
    [SerializeField] TextMeshProUGUI r_infoText = null;

    private bool _apiresult = false;
    private bool _isRequestingRankingData = false;

    private  void Awake()
    {
        OnClickRank();
    }

    private async void OnClickRank()
    {
        if (_isRequestingRankingData) return;
        _isRequestingRankingData = true;

        _apiresult = await APIManager.Instance.GetRankingAPI();

        r_infoText.gameObject.SetActive(false);

        if (_apiresult)
        {
            GameData userInfo = APIManager.Instance.GetApiSODicUerData();

            if (userInfo == null)
            {
                r_infoText.gameObject.SetActive(true);
                r_infoText.text = "Please Log in";
            }

            List<RankingData> rankingDataList = APIDataSO.Instance.GetValueByKey<List<RankingData>>("RankingData");

            if (rankingDataList != null && rankingDataList.Count > 0)
            {
                for (int i = 0; i < rankingDataList.Count; i++)
                {
                    RankingData rankingData = rankingDataList[i];

                    if (i < rankTopThreeName.Length)
                    {
                        rankTopThreeName[i].text = rankingData.id;
                    }

                    if (i < rankTopTen.Length)
                    {
                        rankTopTen[i].text = $"ID: {rankingData.id}, Score: {rankingData.score}, Rank: {rankingData.ranking}";
                    }

                    if (rankingDataList.Count == 11)
                    {
                        userRank.text = $"ID: {rankingData.id}, Score: {rankingData.score}, Rank: {rankingData.ranking}";
                    }
                    else if (rankingDataList.Count == 10)
                    {
                        RankingData userRankingData = rankingDataList.Find(r => r.id == userInfo.ID);
                        if (userRankingData != null)
                        {
                            userRank.text = $"ID: {userRankingData.id}, Score: {userRankingData.score}, Rank: {userRankingData.ranking}";
                        }
                        else
                        {
                            Debug.LogError("User's ranking data not found.");
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No ranking data found in APIDataSO.");
        }

        _isRequestingRankingData = false;
    }

    public void OnClickRankBackBut()
    {
        for (int i = 0; i < rankTopThreeName.Length; ++i)
        {
            rankTopThreeName[i].text = "";
            rankTopThreeName[i].text = "";
            userRank.text = "";
        }

        for(int i = 0; i < rankTopTen.Length; ++ i)
        {
            rankTopTen[i].text = "";
        }

        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnClickRank();
    }

    private void OnDisable()
    {
        APIDataSO.Instance.RemoveValueByKey(APIDataDicKey.RankingData);
    }
}