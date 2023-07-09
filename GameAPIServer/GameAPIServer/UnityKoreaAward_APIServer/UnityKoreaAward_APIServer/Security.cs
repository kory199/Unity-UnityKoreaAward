using System.Security.Cryptography;
using System.Text;

namespace UnityKoreaAward_APIServer;

public class Security
{
    private const String AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";


    public static string MakeHashingPassWord(String saltValue, String pw)
    {
        var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(saltValue + pw));
        var stringBuilder = new StringBuilder();

        foreach(var n in hash)
        {
            stringBuilder.AppendFormat("{0:x2}", n);
        }

        return stringBuilder.ToString();
    }

    public static string SaltString()
    {
        var bytes = new Byte[64];

        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }

        return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
    }
}