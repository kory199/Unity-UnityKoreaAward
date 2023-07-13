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
using System.Reflection;

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

        bool isEnum = false;
        List<string> properties = new List<string>();
        foreach (var property in firstObject.Properties())
        {
            string candidatePropertyName = property.Name.EndsWith("Type") ? property.Name : null;
            // Check if property has a corresponding Enum type
            Type enumType = null;
            if (!string.IsNullOrEmpty(candidatePropertyName))
            {
                enumType = typeof(EnumTypes).GetNestedType(candidatePropertyName, BindingFlags.Public);
            }

            // If an Enum type is found and the value is a string, try converting
            if (enumType != null && property.Value.Type == JTokenType.String)
            {
                string type = enumType.Name;
                properties.Add($"    public {type} {property.Name};");
                isEnum = true;
            }
            else
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
                else if (property.Value.Type == JTokenType.Boolean)
                {
                    type = "bool";
                }
                else if (property.Value.Type == JTokenType.Array) // 추가된 배열 처리 부분
                {
                    JArray arrayValue = (JArray)property.Value;
                    JToken firstArrayElement = arrayValue.FirstOrDefault();

                    if (firstArrayElement != null)
                    {
                        if (firstArrayElement.Type == JTokenType.Integer)
                            type = "List<int>";
                        else if (firstArrayElement.Type == JTokenType.Float)
                            type = "List<float>";
                        else if (firstArrayElement.Type == JTokenType.String)
                            type = "List<string>";
                        else
                            throw new NotSupportedException($"지원되지 않는 배열 요소 형식: {firstArrayElement.Type}");
                    }
                    else
                    {
                        type = "List<object>";
                    }
                }
                properties.Add($"    public {type} {property.Name};");
            }
        }
        string usingEnum = true == isEnum ? "using static EnumTypes;" : string.Empty;
        string classTemplate = $@"using System.Collections.Generic;
{usingEnum}

[System.Serializable]
public class {className}
{{
{string.Join("\n", properties)}
    public static Dictionary<int, {className}> table = new Dictionary<int, {className}> ();   
}}";

        return classTemplate;
    }
}
