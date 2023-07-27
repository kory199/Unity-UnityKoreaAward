using System.Collections.Generic;
using APIModels;
using UnityEditor;

[CustomEditor(typeof(APIDataSO))]
public class APIDataSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        APIDataSO myTarget = (APIDataSO)target;

        foreach(var pair in myTarget.responseDataDic)
        {
            EditorGUILayout.LabelField("Key : ", pair.Key);

            if (pair.Value is GameData gameData)
            {
                EditorGUILayout.LabelField("ID: ", gameData.ID);
                EditorGUILayout.LabelField("AuthToken: ", gameData.AuthToken);
            }
            else if (pair.Value is List<PlayerData> playerDataList)
            {
                for (int i = 0; i < playerDataList.Count; i++)
                {
                    PlayerData playerData = playerDataList[i];
                    EditorGUILayout.LabelField("Player " + (i + 1) + " ID: ", playerData.id.ToString());
                    EditorGUILayout.LabelField("Exp: ", playerData.exp.ToString());
                    EditorGUILayout.LabelField("HP: ", playerData.hp.ToString());
                    EditorGUILayout.LabelField("Score: ", playerData.score.ToString());
                    EditorGUILayout.LabelField("Level: ", playerData.level.ToString());
                    EditorGUILayout.LabelField("Status: ", playerData.status.ToString());
                }
            }
            else if (pair.Value is List<RankingData> rankingDataList)
            {
                for (int i = 0; i < rankingDataList.Count; i++)
                {
                    RankingData rankingData = rankingDataList[i];
                    EditorGUILayout.LabelField("Player " + (i + 1) + " ID: ", rankingData.id);
                    EditorGUILayout.LabelField("Score: ", rankingData.score.ToString());
                    EditorGUILayout.LabelField("Rank: ", rankingData.ranking.ToString());
                }
            }
            else
            {
                EditorGUILayout.LabelField("Value : ", pair.Value.ToString());
            }
        }

        base.OnInspectorGUI();
    }
}