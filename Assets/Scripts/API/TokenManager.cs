using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TokenManager : MonoSingleton<TokenManager>
{
    private const string IdKey = "ID";
    private const string AuthTokenKey = "AuthToken";

    private void Save(string key, string value)
    {
        string encryptedValue = Convert.ToBase64String(SecureAES.Encrypt(value));
        PlayerPrefs.SetString(key, encryptedValue);
    }

    private string Get(string key)
    {
        string encryptedValue = PlayerPrefs.GetString(key);

        if(string.IsNullOrEmpty(encryptedValue)) return null;

        byte[] encryptedBytes = Convert.FromBase64String(encryptedValue);
        return SecureAES.Decrypt(encryptedBytes);
    }

    private void Delete(string key) => PlayerPrefs.DeleteKey(key);

    // === ID ===
    public void SaveID(string id) => Save(IdKey, id);
    public string GetID() => Get(IdKey);
    public void DeleteID() => Delete(IdKey);

    // === Token === 
    public void SaveToken(string token) => Save(AuthTokenKey, token);
    public string GetToken() => Get(AuthTokenKey);
    public void DeleteToken() => Delete(AuthTokenKey);

    public void DeleteAll() => PlayerPrefs.DeleteAll();
}