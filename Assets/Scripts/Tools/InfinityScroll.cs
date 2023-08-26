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
public class InfinityScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect = null;
    [SerializeField] private List<RectTransform> _contents = null;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup = null;
    [SerializeField] private RectTransform _contentPivot = null;
    [SerializeField] private RectTransform _viewPort = null;
    [SerializeField] private RectTransform _scrollRectTransform = null;
    [SerializeField] private RawImage _testImage = null;
    [SerializeField] private RawImage _baseImage = null;
    [SerializeField] private List<ContentsInfo> _contentsInfo = new List<ContentsInfo>();
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
    private void Awake()
    {
        _count = _contents.Count;
        _scrollViewPosY = _scrollRect.viewport.position.y;

        _itemSizeX = _contents[0].rect.width;
        _itemSizeY = _contents[0].rect.height;
        _spacing = _verticalLayoutGroup.spacing;

        _initPosY = _scrollRectTransform.transform.position.y + _scrollRectTransform.rect.height * 0.5f;
        _itemLastPosY = _scrollRectTransform.transform.position.y - _scrollRectTransform.rect.height * 0.5f - (_itemSizeY + _spacing);
        //SetData();
        //이후 enum타입으로 뷰포트 맞추게 변경 요망
        //현재 상단 맞춤
        /*  _contentPivot.anchorMin = new Vector2(0, 1);
          _contentPivot.anchorMax = new Vector2(1, 1);
          _contentPivot.pivot = new Vector2(0, 1);*/
    }
    private void Start()
    {
        // Content의 크기를 변경
        _contentPivot.sizeDelta = new Vector2(_contentPivot.sizeDelta.x, (_contentsInfo.Count) * (_itemSizeY + _spacing));

        // ScrollView의 높이를 변경된 Content에 맞게 조절
        _scrollRect.normalizedPosition = new Vector2(_scrollRect.normalizedPosition.x, 1); // 스크롤 위치를 맨 위로 조정
    }

    private void Update()
    {
        #region base scroll
        /*
                //하단 스크롤
                if (_nowIDX < _contentsInfo.Count - _count - 1)
                {
                    if (_contents[0].position.y > _scrollViewPosY + _itemSizeY + _spacing)
                    {
                        _contents[0].position = new Vector2(_contents[0].position.x, _contents[_contents.Count - 1].position.y - (_itemSizeY + _spacing));

                        RectTransform tempRect = _contents[0];
                        TextMeshProUGUI tempText = _contentsText[0];
                        RawImage tempImage = _contentsImage[0];
                        _contents[0].transform.SetAsLastSibling();
                        for (int i = 0; i < _count - 1; i++)
                        {
                            _contents[i] = _contents[i + 1];
                            _contentsText[i] = _contentsText[i + 1];
                            _contentsImage[i] = _contentsImage[i + 1];
                        }
                        _contents[_count - 1] = tempRect;
                        _contentsText[_count - 1] = tempText;
                        _contentsImage[_count - 1] = tempImage;

                        _nowIDX++;

                        _contentsImage[_count - 1].texture = _contentsInfo[_nowIDX + _count].RawImage.texture;
                        _contentsText[_count - 1].text = _contentsInfo[_nowIDX + _count].Text;
                    }
                }

                if (_nowIDX > 0)
                {
                    //상단 스크롤
                    if (_contents[_contents.Count - 1].position.y < _itemLastPosY)
                    {
                        _contents[_contents.Count - 1].position = new Vector2(_contents[_contents.Count - 1].position.x, _contents[0].position.y + (_itemSizeY + _spacing));

                        RectTransform tempRect = _contents[_count - 1];
                        TextMeshProUGUI tempText = _contentsText[_count - 1];
                        RawImage tempImage = _contentsImage[_count - 1];
                        _contents[_count - 1].transform.SetAsFirstSibling();

                        for (int i = _contents.Count - 1; i > 0; i--)
                        {
                            _contents[i] = _contents[i - 1];
                            _contentsText[i] = _contentsText[i - 1];
                            _contentsImage[i] = _contentsImage[i - 1];

                        }
                        _contents[0] = tempRect;
                        _contentsText[0] = tempText;
                        _contentsImage[0] = tempImage;
                        _nowIDX--;

                        _contentsImage[0].texture = _contentsInfo[_nowIDX].RawImage.texture;
                        _contentsText[0].text = _contentsInfo[_nowIDX].Text;
                    }
                }
        */
        #endregion

        //하단 스크롤
        if (_nowTopIDX < _contentsInfo.Count - _count)
        {
            if (_contents[0].position.y > _initPosY + _itemSizeY + _spacing)
            {
                _contents[0].position = new Vector2(_contents[0].position.x, _contents[_count - 1].position.y - (_itemSizeY + _spacing));


                RectTransform tempRect = _contents[0];
                TextMeshProUGUI tempText = _contentsText[0];
                RawImage tempImage = _contentsImage[0];
                for (int i = 0; i < _count - 1; i++)
                {
                    _contents[i] = _contents[i + 1];
                    _contentsText[i] = _contentsText[i + 1];
                    _contentsImage[i] = _contentsImage[i + 1];
                }
                _contents[_count - 1] = tempRect;
                _contentsText[_count - 1] = tempText;
                _contentsImage[_count - 1] = tempImage;

                _nowTopIDX++;

                //다음 범위 가져올 데이터 가공부분
                _contentsImage[_count - 1].texture = _contentsInfo[_nowTopIDX + _count - 1].RawImage.texture;
                if (_contentsInfo[_nowTopIDX + _count - 1].Bool)
                {
                    _contentsImage[_count - 1].color = Color.green;
                }
                else
                {
                    _contentsImage[_count - 1].color = Color.red;
                }
                _contentsText[_count - 1].text = _contentsInfo[_nowTopIDX + _count - 1].Text;
            }
        }

        if (_nowTopIDX > 0)
        {
            //상단 스크롤
            if (_contents[_contents.Count - 1].position.y < _itemLastPosY)
            {
                _contents[_contents.Count - 1].position = new Vector2(_contents[_contents.Count - 1].position.x, _contents[0].position.y + (_itemSizeY + _spacing));

                RectTransform tempRect = _contents[_count - 1];
                TextMeshProUGUI tempText = _contentsText[_count - 1];
                RawImage tempImage = _contentsImage[_count - 1];

                for (int i = _contents.Count - 1; i > 0; i--)
                {
                    _contents[i] = _contents[i - 1];
                    _contentsText[i] = _contentsText[i - 1];
                    _contentsImage[i] = _contentsImage[i - 1];

                }
                _contents[0] = tempRect;
                _contentsText[0] = tempText;
                _contentsImage[0] = tempImage;
                _nowTopIDX--;

                //이전 범위 가져올 데이터 가공부분
                _contentsImage[0].texture = _contentsInfo[_nowTopIDX].RawImage.texture;
                if (_contentsInfo[_nowTopIDX].Bool)
                {
                    _contentsImage[0].color = Color.green;
                }
                else
                {
                    _contentsImage[0].color = Color.red;
                }
                _contentsText[0].text = _contentsInfo[_nowTopIDX].Text;
            }
        }
    }

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
    public void SetData(List<KeyValuePair<string, bool>> sensorList)
    {
        if (_contentsInfo.Count == 0)
        {
            for (int i = 0; i < sensorList.Count; i++)
            {
                ContentsInfo temp = new ContentsInfo();
                temp.Idx = i;
                temp.Bool = sensorList[i].Value;
                temp.RawImage = _baseImage;
                temp.Text = $"{sensorList[i].Key} : {sensorList[i].Value}";
                _contentsInfo.Add(temp);
            }
            for (int i = 0; i < _count; i++)
            {
                _contentsImage.Add(_contents[i].GetComponent<RawImage>());
                _contentsText.Add(_contents[i].GetComponentInChildren<TextMeshProUGUI>());
                _contentsImage[i].texture = _contentsInfo[i].RawImage.texture;
                _contentsText[i].text = _contentsInfo[i].Text;
                if (_contentsInfo[i].Bool)
                {
                    _contentsImage[i].color = Color.green;
                }
                else
                {
                    _contentsImage[i].color = Color.red;
                }
            }
        }
        else
        {
            for (int i = 0; i < sensorList.Count; i++)
            {
                _contentsInfo[i].Bool = sensorList[i].Value;
            }
            for (int i = _nowTopIDX; i < _nowTopIDX + _contents.Count; i++)
            {
                if (_contentsInfo[i].Bool)
                {
                    _contentsImage[i - _nowTopIDX].color = Color.green;
                }
                else
                {
                    _contentsImage[i - _nowTopIDX].color = Color.red;
                }
            }
        }

    }
}
