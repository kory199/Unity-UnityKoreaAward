using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

public class ChangeFont : EditorWindow
{
    private TMP_FontAsset _changeFont;

    [MenuItem("Tools/ ChangeFont")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChangeFont));
    }

    private void OnGUI()
    {
        _changeFont = EditorGUILayout.ObjectField("Font" , _changeFont, typeof(TMP_FontAsset), false) as TMP_FontAsset;

        GUILayout.Space(30f);

        if(GUILayout.Button("Apply"))
        {
            Scene currentScene = EditorSceneManager.GetActiveScene();
            foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
            {
                if (buildScene.enabled)
                {
                    Scene scene = EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Additive);
                    ChangeFontInScene(scene);
                    EditorSceneManager.CloseScene(scene, true);
                }
            }

            EditorUtility.DisplayDialog("Success", "Font successfully changed!", "OK");
            EditorSceneManager.OpenScene(currentScene.path);
        }
    }

    void ChangeFontInScene(Scene scene)
    {
        GameObject[] allGameObjects = scene.GetRootGameObjects();

        foreach (GameObject gameObject in allGameObjects)
        {
            ChangeFontInGameObject(gameObject);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    void ChangeFontInGameObject(GameObject gameObject)
    {
        TMP_InputField[] inputFields = gameObject.GetComponentsInChildren<TMP_InputField>();
        foreach (TMP_InputField inputField in inputFields)
        {
            inputField.fontAsset = _changeFont;
        }

        TextMeshProUGUI[] textComponents = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textComponent in textComponents)
        {
            textComponent.font = _changeFont;
        }

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            ChangeFontInGameObject(gameObject.transform.GetChild(i).gameObject);
        }
    }
}