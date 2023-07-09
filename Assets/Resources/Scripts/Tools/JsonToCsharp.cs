using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using static Unity.PlasticSCM.Editor.WebApi.CredentialsResponse;
using Newtonsoft.Json;
using System;
using System.Collections;

public class JsonToCSharp : EditorWindow
{
    [MenuItem("Tools/Generate C# Classes from JSON Files")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(JsonToCSharp));
    }
    private void OnGUI()
    {
        GUILayout.Label("Generate C# Classes from JSON Files", EditorStyles.boldLabel);
        if (GUILayout.Button("Generate"))
        {
            GenerateClassesFromJson();
        }
    }

    private void GenerateClassesFromJson()
    {
        string resourcesPath = Path.Combine(Application.dataPath, "Resources", "ResourcesTable");
        DirectoryInfo directoryInfo = new DirectoryInfo(resourcesPath);

        foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
        {
            string className = Path.GetFileNameWithoutExtension(file.Name);

            JArray jsonArray;
            try
            {
                jsonArray = JArray.Parse(File.ReadAllText(file.FullName));
            }
            catch (JsonReaderException)
            {
                Debug.LogError($"Invalid JSON file: {file.FullName}");
                continue;
            }

            string classContent = GenerateClass(className, jsonArray);

            string csharpFilePath = Path.Combine("Assets/Scripts/Tables", className + ".cs");
            Debug.Log($"Generate Successed {csharpFilePath}");
            File.WriteAllText(csharpFilePath, classContent);
        }

        AssetDatabase.Refresh();
    }

    private string GenerateClass(string className, JArray jsonArray)
    {
        JObject firstObject = (JObject)jsonArray.FirstOrDefault();
        if (firstObject == null)
        {
            Debug.LogError($"Empty JSON array for class {className}");
            return string.Empty;
        }

        List<string> properties = new List<string>();
        foreach (var property in firstObject.Properties())
        {
            string type = "object";

            if (property.Value.Type == JTokenType.String)
            {
                type = "string";
            }
            else if (property.Value.Type == JTokenType.Integer)
            {
                type = "int";
            }
            else if (property.Value.Type == JTokenType.Float)
            {
                type = "float";
            }

            properties.Add($"    public {type} {property.Name};");
        }

        string classTemplate = $@"using System.Collections.Generic;

[System.Serializable]
public class {className}
{{
{string.Join("\n", properties)}
    public static Dictionary<int, {className}> table = new Dictionary<int, {className}> ();   
}}";

        return classTemplate;
    }
}