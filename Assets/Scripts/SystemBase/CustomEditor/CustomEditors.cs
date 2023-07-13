using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StringLoacalizer))]
public class CustomStringLoacalizerEditors : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StringLoacalizer updateTrigger = (StringLoacalizer)target;

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
