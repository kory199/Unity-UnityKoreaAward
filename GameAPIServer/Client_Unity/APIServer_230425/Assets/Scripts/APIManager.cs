using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using static System.Net.WebRequestMethods;

[Serializable]
public class User
{
    public string ID;
    public string Password;
}

public class APIManager : MonoBehaviour
{
    #region
    private static APIManager instance;
    public static APIManager Inst
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<APIManager>();

                if(instance ==null)
                {
                    instance = new GameObject(nameof(APIManager), typeof(APIManager)).GetComponent<APIManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    public void StartLoginCheck()
    {
        User user = new User
        {
            ID = UIManager.Inst.idText.text,
            Password = UIManager.Inst.pwText.text
        };

        print($"id : {user.ID} , pw : {user.Password}");

        string json = JsonUtility.ToJson(user);

        StartCoroutine(GetRequest(json));
    }

    IEnumerator GetRequest(string json)
    {
        string loginUrl = "http://localhost:11500/Login";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(loginUrl, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            print("jsonToSend" + jsonToSend);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 요청 보내기
            yield return request.SendWebRequest();
        }
    }

    public void CreateAccount()
    {
        User user = new User
        {
            ID = UIManager.Inst.create_idInput.text,
            Password = UIManager.Inst.create_pwInput.text
        };

        print($"[CreateAccount] Id:{user.ID},  pw : {user.Password}");

        string json = JsonUtility.ToJson(user);

        StartCoroutine(CreateAccountRequest(json));
    }

    IEnumerator CreateAccountRequest(string json)
    {
        string createAccountUrl = "http://localhost:11500/CreateAccount";

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(createAccountUrl, json))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            print("jsonToSend" + jsonToSend);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            print($"request : {request.SendWebRequest().ToString()}");

            //UIManager.Inst.requestInfo.text = request.SendWebRequest().ToString();
            // 요청 보내기
            yield return request.SendWebRequest();

        }
    }
}
