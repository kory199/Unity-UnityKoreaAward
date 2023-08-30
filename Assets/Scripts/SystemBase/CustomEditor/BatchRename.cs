using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class BatchRename : ScriptableWizard
{
    public string BaseName = "";
    public int StartNum = 0;
    public int Increment = 1;

    [MenuItem("Tools/ Batch Rename")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Barch Rename", typeof(BatchRename), "Rename");
    }

    // Invoked when the window first appears
    private void OnEnable() 
    {
        UpdateSelectionHelper();
    }

    // Invoked when the selection in the scene changes
    private void OnSelectionChange() 
    {
        UpdateSelectionHelper();
    }

    private void UpdateSelectionHelper()
    {
        if (Selection.objects != null)
        {
            Debug.Log("Number of objects selected: " + Selection.objects.Length);
        }
    }

    private void OnWizardCreate()
    {
        // If no object is selected, exit
        if (Selection.objects == null) return;

        int PostFix = StartNum;

        foreach(Object obj in Selection.objects)
        {
            obj.name = BaseName + PostFix;
            PostFix += Increment;
        }
    }
}
#endif
