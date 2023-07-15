using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StringLocalizer : MonoBehaviour
{
    
    public int id = -1;

    private TextMeshProUGUI _textMeshProUGUI = null;
    private Dictionary<int,StringData> _stringData = null;
    private UIManager _uiManager = null;
    private bool _isInit = false;

    private async void Awake() => await Setup();
    private async UniTask Setup()
    {
        await UniTask.WaitUntil(() => null != StringData.table);
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        _stringData = StringData.table;
        _uiManager = UIManager.Instance;

        _uiManager.onChangeLanguage += StringUpdate;
        _isInit = true;
    }

    private void Start() => _= StringUpdate(id);

    public async UniTask<bool> StringUpdate(int argId)
    {
        id = argId;
        await UniTask.WaitUntil(() => false != _isInit);
        var text = CheckLanguage(argId);
        if (true == string.IsNullOrEmpty(text))
        {
            string nullString = IfNullData();
            _textMeshProUGUI.text = nullString;
            Debug.LogError($"{GetType()} NullReferenceException :: Can not found string :: Id : {argId}");
            return false;
        }
        _textMeshProUGUI.text = text;
        return true;
    }

    public async void StringUpdate()
    {
        await UniTask.WaitUntil(() => false != _isInit);
        var text = CheckLanguage(id);
        if (true == string.IsNullOrEmpty(text))
        {
            string nullString = IfNullData();
            _textMeshProUGUI.text = nullString;
            Debug.LogError($"{GetType()} NullReferenceException :: Can not found string :: Id : {id}");
            return ;
        }
        _textMeshProUGUI.text = text;
    }

    private string IfNullData()
    {
        switch (_uiManager.language)
        {
            case EnumTypes.Language.Eng:
                return _stringData[1].Eng;
            case EnumTypes.Language.Kor:
                return _stringData[1].Kor;
        }
        return _stringData[1].Eng;
    }

    public string CheckLanguage(int argId)
    {
        if (false == _stringData.ContainsKey(argId))
            return string.Empty;

        switch (_uiManager.language)
        {
            case EnumTypes.Language.Eng:
                return _stringData[argId].Eng;
            case EnumTypes.Language.Kor:
                return _stringData[argId].Kor;
        }

        return string.Empty;
    }

    #region Custom Editor
    public async UniTask OnClickEditorButton()
    {
        JsonLoader loader = new JsonLoader();
        loader.Load(nameof(StringData));

        await UniTask.WaitUntil(() => 0 != StringData.table.Count);
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        _stringData = StringData.table;
        _uiManager = UIManager.Instance;
        StringEditorUpdate();
    }

    public void StringEditorUpdate()
    {
        if (false == _stringData.ContainsKey(id))
        {
            _textMeshProUGUI.text = _stringData[1].Eng;
            Debug.LogError($"{GetType()} NullReferenceException :: Can not found string :: Id : {id}");
            return;
        }

        _textMeshProUGUI.text = _stringData[id].Eng;
    }
    #endregion
}
