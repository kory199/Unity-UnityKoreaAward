using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class SecretManager
{
    private Dictionary<string, byte[]> secretsDic = new();
    private Dictionary<string, byte[]> previousSecretsDic = new();

    public byte[] Get(string key)
    {
        if(!secretsDic.ContainsKey(key))
        {
            GenerateAndSaveNewKey(key);
        }

        return secretsDic[key];
    }

    public byte[] GetPrevious(string key)
    {
        if (previousSecretsDic.ContainsKey(key))
        {
            return previousSecretsDic[key];
        }

        return null;
    }

    public void Set(string key, byte[] secret)
    {
        if (secretsDic.ContainsKey(key))
        {
            previousSecretsDic[key] = secretsDic[key];
        }
        else
        {
            previousSecretsDic[key] = null;
        }

        secretsDic[key] = secret;
    }

    public bool HasChanged(string key)
    {
        byte[] current = Get(key);
        byte[] previous = GetPrevious(key);

        if (current == null || previous == null)
        {
            return current != previous;
        }

        return !current.SequenceEqual(previous);
    }

    public void GenerateAndSaveNewKey( string key)
    {
        byte[] newKey = new byte[16];

        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(newKey);
        }

        Set(key, newKey);
    }
}