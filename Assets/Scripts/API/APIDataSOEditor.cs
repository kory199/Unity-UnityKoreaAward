using System.Collections;
using System.Collections.Generic;
using APIModels;
using UnityEditor;

[System.Serializable]
public class InspectorDictionaryElement
{
    public string key;
    public string valueName;
    public object value;

    public InspectorDictionaryElement(string key, string valueName, object value)
    {
        this.key = key;
        this.valueName = valueName;
        this.value = value;
    }
}

[CustomEditor(typeof(APIDataSO))]
public class APIDataSOEditor : Editor
{
    private List<InspectorDictionaryElement> dictionaryElements = new List<InspectorDictionaryElement>();

    private void OnEnable()
    {
        EditorApplication.update += Update;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    private void Update()
    {
        APIDataSO myTarget = (APIDataSO)target;
        dictionaryElements.Clear();

        var gameData = new GameData();
        var defaultPlayerData = new PlayerData();

        if (myTarget.responseDataDic != null && myTarget.responseDataDic.Count > 0)
        {
            bool hasGameData = false;
            bool hasPlayerData = false;

            foreach (KeyValuePair<string, object> pair in myTarget.responseDataDic)
            {
                string valueTypeName = pair.Value != null ? pair.Value.GetType().Name : "null";

                if (pair.Value is List<RankingData>)
                {
                    valueTypeName = "List<RankingData>";
                }

                if (valueTypeName == "GameData")
                {
                    hasGameData = true;
                    dictionaryElements.Add(new InspectorDictionaryElement(pair.Key, valueTypeName, pair.Value));
                }
                else if (valueTypeName == "PlayerData")
                {
                    hasPlayerData = true;
                    dictionaryElements.Add(new InspectorDictionaryElement(pair.Key, valueTypeName, pair.Value));
                }
                else if (valueTypeName == "List<RankingData>")
                {
                    dictionaryElements.Add(new InspectorDictionaryElement(pair.Key, valueTypeName, pair.Value));
                }
            }

            if (!hasGameData)
            {
                dictionaryElements.Add(new InspectorDictionaryElement("GameData", "GameData", gameData));
            }

            if (!hasPlayerData)
            {
                dictionaryElements.Add(new InspectorDictionaryElement("PlayerData", "PlayerData", defaultPlayerData));
            }

        }
        else
        {
            dictionaryElements.Add(new InspectorDictionaryElement("GameData", "GameData", gameData));
            dictionaryElements.Add(new InspectorDictionaryElement("PlayerData", "PlayerData", defaultPlayerData));
        }

        EditorApplication.QueuePlayerLoopUpdate();
        Repaint();
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        APIDataSO myTarget = (APIDataSO)target;

        serializedObject.Update();

        for (int i = 0; i < dictionaryElements.Count; i++)
        {
            var element = dictionaryElements[i];
            EditorGUILayout.LabelField("Key", element.key);

            if (element.value == null)
            {
                EditorGUILayout.LabelField("Value: null");
                continue;
            }

            if (element.valueName == "GameData")
            {
                var gameData = element.value as GameData;
                gameData.ID = EditorGUILayout.TextField("ID", gameData.ID);
                gameData.AuthToken = EditorGUILayout.TextField("AuthToken", gameData.AuthToken);
            }
            else if (element.valueName == "PlayerData")
            {
                var playerData = element.value as PlayerData;
                EditorGUILayout.Space();
                playerData.id = EditorGUILayout.TextField("ID", playerData.id);
                playerData.exp = EditorGUILayout.IntField("Exp", playerData.exp);
                playerData.hp = EditorGUILayout.IntField("HP", playerData.hp);
                playerData.score = EditorGUILayout.IntField("Score", playerData.score);
                playerData.status = EditorGUILayout.IntField("Status", playerData.status);
            }
            else if (element.valueName.Contains("List"))
            {
                var dataList = element.value as IList;

                if (dataList != null)
                {
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        var rankingData = dataList[j] as RankingData;
                        if (rankingData != null)
                        {
                            EditorGUILayout.Space();
                            rankingData.id = EditorGUILayout.TextField("ID: ", rankingData.id);
                            rankingData.score = EditorGUILayout.IntField("Score: ", rankingData.score);
                            rankingData.ranking = EditorGUILayout.IntField("Rank: ", rankingData.ranking);
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("Value: ", element.value.ToString());
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}