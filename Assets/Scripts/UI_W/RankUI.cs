using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    private float debounceTime = 0.5f;
    private float lastAPICallTime;

    private void Awake()
    {
        //OnClickRank();
        r_infoText.gameObject.SetActive(true);
        r_infoText.text = "Please Wait ...";
    }

    private async void OnClickRank()
    {
        float currentTime = Time.time;

        if (currentTime - lastAPICallTime < debounceTime)
        {
            return;
        }

        lastAPICallTime = currentTime;

        (List<RankingData> rankingDataList, string _id)= await APIManager.Instance.GetRankingAPI();

        r_infoText.gameObject.SetActive(false);

        try
        {
            if (rankingDataList == null)
            {
                r_infoText.gameObject.SetActive(true);
                r_infoText.text = "Please Login ...";
            }
            else
            {

                ClearRankData();

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
                        RankingData userRankingData = rankingDataList.Find(r => r.id == _id);

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
        catch (TaskCanceledException)
        {
            Debug.Log("API call was cancelled.");
        }
    }

    private void ClearRankData()
    {
        for (int i = 0; i < rankTopThreeName.Length; ++i)
        {
            rankTopThreeName[i].text = "";
        }
        userRank.text = "";

        for (int i = 0; i < rankTopTen.Length; ++i)
        {
            rankTopTen[i].text = "";
        }
    }

    public void OnClickRankBackBut()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OnClickRank();
    }
}