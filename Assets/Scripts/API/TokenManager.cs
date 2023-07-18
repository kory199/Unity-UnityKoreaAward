using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenManager : MonoSingleton<TokenManager>
{
    private const string AuthTokenKey = "AuthToken";


    public void SaveToken(string token)
    {
        PlayerPrefs.SetString(AuthTokenKey, token);
    }

    public string GetToken() => PlayerPrefs.GetString(AuthTokenKey);

    public void DeleteToken() => PlayerPrefs.DeleteKey(AuthTokenKey);
}