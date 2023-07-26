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

            else if (pair.Value is PlayerData playerData)
            {
                EditorGUILayout.LabelField("Player UID: ", playerData.player_uid.ToString());
                EditorGUILayout.LabelField("Exp: ", playerData.exp.ToString());
                EditorGUILayout.LabelField("HP: ", playerData.hp.ToString());
                EditorGUILayout.LabelField("Score: ", playerData.score.ToString());
                EditorGUILayout.LabelField("Level: ", playerData.level.ToString());
                EditorGUILayout.LabelField("Status: ", playerData.status.ToString());
            }

            else if (pair.Value is RankingData rankingData)
            {
                EditorGUILayout.LabelField("ID ", rankingData.rankId);
                EditorGUILayout.LabelField("Score ", rankingData.rankScore.ToString());
                EditorGUILayout.LabelField("Rank ", rankingData.ranking.ToString());
            }

            else
            {
                EditorGUILayout.LabelField("Value : ", pair.Value.ToString());
            }
        }

        base.OnInspectorGUI();
    }
}