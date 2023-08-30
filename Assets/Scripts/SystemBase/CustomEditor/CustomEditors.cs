using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(StringLocalizer))]
#endif
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
#endif
