using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

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
            ChangedFont();
        }
    }

    void ChangedFont()
    {
        TMP_InputField[] inputFields = Resources.FindObjectsOfTypeAll<TMP_InputField>();

        foreach (TMP_InputField inputField in inputFields)
        {
            inputField.fontAsset = _changeFont;
        }

        TextMeshProUGUI[] textComponents = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        foreach (TextMeshProUGUI textComponent in textComponents)
        {
            textComponent.font = _changeFont;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorUtility.DisplayDialog("Success", "Font successfully changed!", "OK");
    }
}