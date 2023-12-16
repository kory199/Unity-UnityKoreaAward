using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankUI : UIBase
{
    [SerializeField] TextMeshProUGUI[] rankTopThreeName = null;
    [SerializeField] TextMeshProUGUI[] rankTopTen = null;
    [SerializeField] TextMeshProUGUI userRank = null;
    [SerializeField] TextMeshProUGUI r_infoText = null;
    [Header("Scroll")]
    [SerializeField] private TextMeshProUGUI[] _rankNickName = null;
    [SerializeField] private TextMeshProUGUI[] _rankScore = null;
    [SerializeField] private TextMeshProUGUI[] _rankNumber = null;
    [SerializeField] private RawImage[] _rankImage = null;
    [SerializeField] private Sprite _baseImage = null;


    private float debounceTime = 0.1f;
    private float lastAPICallTime;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }
    protected override void Awake()
    {
        r_infoText.gameObject.SetActive(true);
        r_infoText.text = "Please Wait ...";
        foreach(var image in _rankImage)
            image.texture = _baseImage.texture;
    }

    private async void OnClickRank()
    {
        float currentTime = Time.time;

        if (currentTime - lastAPICallTime < debounceTime)
        {
            return;
        }

        lastAPICallTime = currentTime;

        List<RankingData> rankingDataList = new();

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
                for (int i = 0; i < rankingDataList.Count; ++i)
                {
                    _rankNickName[i].text = rankingDataList[i].id;
                    _rankScore[i].text = rankingDataList[i].score.ToString();
                    _rankNumber[i].text = rankingDataList[i].ranking.ToString();
                 
                }
                ClearRankData();
                for (int i = 0; i < rankTopThreeName.Length; ++i)
                {
                    rankTopThreeName[i].text = rankingDataList[i].id;
                }

                //RankingData userRankingData = rankingDataList.Find(r => r.id == _id);

                //if (userRankingData != null)
                //{
                //    userRank.text = $"ID: {userRankingData.id}, Score: {userRankingData.score}, Rank: {userRankingData.ranking}";
                //}
                //else
                //{
                //    Debug.LogError("User's ranking data not found.");
                //}

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
    }
    public UI_SceneLobby uI_SceneLobby = null;
    public void OnClickRankBackBut()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        uI_SceneLobby.OnShow();
        OnHide();
    }

    private void OnEnable()
    {
        OnClickRank();
    }
}