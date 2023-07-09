using TMPro;
using UnityEditor;
using UnityEngine;

public class ChangeFont : EditorWindow
{
    private TMP_FontAsset changeFont;

    [MenuItem("Tool/ ChangeFont")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChangeFont));
    }

    void OnGUI()
    {
        changeFont = EditorGUILayout.ObjectField("Font", changeFont, typeof(TMP_FontAsset), false) as TMP_FontAsset;

        GUILayout.Space(30f);

        if(GUILayout.Button("Apply"))
        {
            ChangedFont();
        }
    }

    void ChangedFont()
    {
        TMP_InputField[] inputFields = Resources.FindObjectsOfTypeAll<TMP_InputField>();
        foreach(TMP_InputField inputField in inputFields)
        {
            inputField.fontAsset = changeFont;
        }

        TextMeshProUGUI[] textComponents = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        foreach(TextMeshProUGUI textComponent in textComponents)
        {
            textComponent.font = changeFont;
        }
    }
}