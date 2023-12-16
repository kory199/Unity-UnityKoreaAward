using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ContentsInfo
{
    public int Idx;
    public RawImage RawImage;
    public string Text;
    public bool Bool;
}
[System.Serializable]
public class RankingInfo
{
    public int Idx;

    public RawImage RankBacGround;
    public string Id;
    public int Score;
    public int Ranking;
}
public class InfinityScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect = null;
    [SerializeField] private List<RectTransform> _contents = null;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup = null;
    [Header("===")]
    [SerializeField] private RectTransform _contentPivot = null;
    [SerializeField] private RectTransform _viewPort = null;
    [Header("===")]
    [SerializeField] private RectTransform _scrollRectTransform = null;
    [SerializeField] private RawImage _testImage = null;
    [SerializeField] private RawImage _baseImage = null;
    [SerializeField] private List<ContentsInfo> _contentsInfo = new List<ContentsInfo>();
    [SerializeField] private List<RankingInfo> _RankInfo = new List<RankingInfo>();
    [SerializeField] private int _nowTopIDX = 0;
    [SerializeField] private int _dataCount = 0;
    float _scrollViewHeight;
    float _scrollViewPosY;
    float _itemSizeX;
    float _itemSizeY;
    float _itemLastPosY;
    float _spacing;
    float _initPosY;
    int _count = 0;

    [SerializeField] private List<RawImage> _contentsImage = null;
    [SerializeField] private List<TextMeshProUGUI> _contentsText = null;

    [SerializeField] private List<RawImage> _contentsBG = null;
    [SerializeField] private List<TextMeshProUGUI> _contentsID = null;
    [SerializeField] private List<TextMeshProUGUI> _contentsScore = null;
    [SerializeField] private List<TextMeshProUGUI> _contentsRanking = null;
    private void Awake()
    {
        _count = _contents.Count;
        _scrollViewPosY = _scrollRect.viewport.position.y;

        _itemSizeX = _contents[0].rect.width;
        _itemSizeY = _contents[0].rect.height;
        _spacing = _verticalLayoutGroup.spacing;

        _initPosY = _scrollRectTransform.transform.position.y + _scrollRectTransform.rect.height * 0.5f;
        _itemLastPosY = _scrollRectTransform.transform.position.y - _scrollRectTransform.rect.height * 0.5f - (_itemSizeY + _spacing);
    }
    private void Start()
    {
        SetScrollSize();
    }

    private void Update()
    {
        //�ϴ� ��ũ��
        if (_nowTopIDX < _RankInfo.Count - _count)
        {
            if (_contents[0].position.y > _initPosY + _itemSizeY + _spacing)
            {
                _contents[0].position = new Vector2(_contents[0].position.x, _contents[_count - 1].position.y - (_itemSizeY + _spacing));

                RectTransform tempRect = _contents[0];
                RawImage tempRankBG = _contentsBG[0];
                TextMeshProUGUI tempRanking = _contentsRanking[0];
                TextMeshProUGUI tempID = _contentsID[0];
                TextMeshProUGUI tempScore = _contentsScore[0];
                for (int i = 0; i < _count - 1; i++)
                {
                    _contents[i] = _contents[i + 1]; //contents pos change

                    _contentsBG[i] = _contentsBG[i + 1];
                    _contentsRanking[i] = _contentsRanking[i + 1];
                    _contentsID[i] = _contentsID[i + 1];
                    _contentsScore[i] = _contentsScore[i + 1];
                }
                _contents[_count - 1] = tempRect;

                _contentsBG[_count - 1] = tempRankBG;
                _contentsRanking[_count - 1] = tempRanking;
                _contentsID[_count - 1] = tempID;
                _contentsScore[_count - 1] = tempScore;

                _nowTopIDX++;

                //���� ���� ������ ������ �����κ�
                _contentsBG[_count - 1].texture = _RankInfo[_nowTopIDX + _count - 1].RankBacGround.texture;
               _contentsRanking[_count - 1].text = _RankInfo[_nowTopIDX + _count - 1].Ranking.ToString();
               _contentsScore[_count - 1].text = _RankInfo[_nowTopIDX + _count - 1].Score.ToString();
               _contentsID[_count - 1].text = _RankInfo[_nowTopIDX + _count - 1].Id;
            }
        }

        if (_nowTopIDX > 0)
        {
            //��� ��ũ��
            if (_contents[_contents.Count - 1].position.y < _itemLastPosY)
            {
                _contents[_contents.Count - 1].position = new Vector2(_contents[_contents.Count - 1].position.x, _contents[0].position.y + (_itemSizeY + _spacing));

                RectTransform tempRect = _contents[_count - 1];
                RawImage tempRankBG = _contentsBG[_count - 1];
                TextMeshProUGUI tempRanking = _contentsRanking[_count - 1];
                TextMeshProUGUI tempID = _contentsID[_count - 1];
                TextMeshProUGUI tempScore = _contentsScore[_count - 1];
                for (int i = _contents.Count - 1; i > 0; i--)
                {
                    _contents[i] = _contents[i - 1];

                    _contentsBG[i] = _contentsBG[i - 1];
                    _contentsRanking[i] = _contentsRanking[i - 1];
                    _contentsID[i] = _contentsID[i - 1];
                    _contentsScore[i] = _contentsScore[i - 1];
                }
                _contents[0] = tempRect;

                _contentsBG[0] = tempRankBG;
                _contentsRanking[0] = tempRanking;
                _contentsID[0 ] = tempID;
                _contentsScore[0] = tempScore;

                _nowTopIDX--;

                _contentsBG[0].texture = _RankInfo[_nowTopIDX].RankBacGround.texture;
                _contentsRanking[0].text = _RankInfo[_nowTopIDX ].Ranking.ToString();
                _contentsScore[0].text = _RankInfo[_nowTopIDX].Score.ToString();
                _contentsID[0].text = _RankInfo[_nowTopIDX].Id;
            }
        }
    }

    /// <summary>
    /// ���� 
    /// </summary>
    public void SetData()
    {
        for (int i = 0; i < 100; i++)
        {
            ContentsInfo temp = new ContentsInfo();
            temp.Idx = i;
            temp.RawImage = _testImage;
            temp.Text = $"number : {i}";
            _contentsInfo.Add(temp);
        }
        for (int i = 0; i < _count; i++)
        {
            _contentsImage.Add(_contents[i].GetComponent<RawImage>());
            _contentsText.Add(_contents[i].GetComponentInChildren<TextMeshProUGUI>());
            _contentsImage[i].texture = _contentsInfo[i].RawImage.texture;
            _contentsText[i].text = _contentsInfo[i].Text;
        }
    }
    public void SetData(List<RankingData> rankingData)
    {
        for(int i = 0; i< rankingData.Count;i++)
        {
            RankingInfo temp = new RankingInfo();
            temp.Idx = i;
            temp.RankBacGround = _baseImage;
            temp.Ranking = rankingData[i].ranking;
            temp.Id = rankingData[i].id;
            temp.Score = rankingData[i].score;
            _RankInfo.Add(temp);
        }
        for (int i = 0; i < _count; i++)
        {
            _contentsBG[i].texture = _baseImage.texture;
            _contentsID[i].text = _RankInfo[i].Id;
            _contentsRanking[i].text = _RankInfo[i].Ranking.ToString();
            _contentsScore[i].text = _RankInfo[i].Score.ToString();
        }
    }
    public void SetScrollSize()
    {
        // Content�� ũ�⸦ ����
        _contentPivot.sizeDelta = new Vector2(_contentPivot.sizeDelta.x, (_RankInfo.Count) * (_itemSizeY + _spacing));
        // ScrollView�� ���̸� ����� Content�� �°� ����
        _scrollRect.normalizedPosition = new Vector2(_scrollRect.normalizedPosition.x, 1); // ��ũ�� ��ġ�� �� ���� ����
    }
}
