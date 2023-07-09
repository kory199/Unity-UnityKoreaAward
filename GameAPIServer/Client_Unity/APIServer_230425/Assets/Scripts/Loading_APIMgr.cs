using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using static System.Net.WebRequestMethods;
using UnityEngine.SceneManagement;

[Serializable]
public class VerResponse
{
    public int result;
    public string masterDataVer;
    public string gameVer;
}

public class Loading_APIMgr : MonoBehaviour
{
    private const String requestMessge = "Get Version";

    void Awake()
    {
         StartCoroutine(GetVersion());    
    }

    IEnumerator GetVersion()
    {
        string getVersion = URL.versionUrl;
        string json = JsonUtility.ToJson(requestMessge);

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(getVersion, json) )
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest(); // 응답이 올때까지 기다린다 

            if (request.error == null)
            {
                string text = request.downloadHandler.text;
                VerResponse ver = JsonUtility.FromJson<VerResponse>(text);

                Loading_UIMgr.Inst.SetVerText(ver.gameVer.ToString());

                //print($"request : {text}");
                print($"Result : {ver.result} , masterdataver : {ver.masterDataVer}, gamever: {ver.gameVer}");
            }
            else
            {
                Loading_UIMgr.Inst.asyncLoad.allowSceneActivation = false;
                print($"error :{request.downloadHandler.text}");
            }
        }
    }
}