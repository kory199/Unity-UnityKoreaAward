using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Security.Cryptography;
using UnityEngine;
using System.IO;

public class SecureAES
{
    private static AesManaged aesManaged;
    private static byte[] key;
    private static SecretManager secretManager;

    static SecureAES()
    {
        secretManager = new SecretManager();
        UpdateKey();
    }

    private static void UpdateKey()
    {
        key = secretManager.Get("AesKey");
        aesManaged = new AesManaged();
    }

    public static byte[] Encrypt(string plainText)
    {
        if (secretManager.HasChanged("AesKey"))
        {
            UpdateKey();
        }

        ICryptoTransform encryptor = aesManaged.CreateEncryptor(key, aesManaged.IV);

        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
            }

            byte[] iv = aesManaged.IV;
            byte[] encrypted = ms.ToArray();
            byte[] result = new byte[iv.Length + encrypted.Length];

            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

            return result;
        }
    }

    public static string Decrypt(byte[] encryptedDataWithIv)
    {
        if (secretManager.HasChanged("AesKey"))
        {
            UpdateKey();
        }

        byte[] iv = new byte[aesManaged.BlockSize / 8];
        byte[] cipherText = new byte[encryptedDataWithIv.Length - iv.Length];

        Buffer.BlockCopy(encryptedDataWithIv, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(encryptedDataWithIv, iv.Length, cipherText, 0, cipherText.Length);

        ICryptoTransform decryptor = aesManaged.CreateDecryptor(key, iv);

        using (MemoryStream ms = new MemoryStream(cipherText))
        {
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader sr = new StreamReader(cs))
                {
                    string plainText = sr.ReadToEnd();
                    return plainText;
                }
            }
        }
    }
}