using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StringLocalizer))]
public class CustomStringLoacalizerEditors : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StringLocalizer updateTrigger = (StringLocalizer)target;

        EditorGUILayout.BeginHorizontal();  
        GUILayout.FlexibleSpace(); 

        if (GUILayout.Button("Update", GUILayout.Width(120), GUILayout.Height(30)))
        {
            _= updateTrigger.OnClickEditorButton();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal(); 

    }
}
