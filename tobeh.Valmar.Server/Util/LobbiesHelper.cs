using System.Security.Cryptography;
using System.Text;

namespace tobeh.Valmar.Server.Util;

public static class LobbiesHelper
{
    private static RSA _rsa = new RSACryptoServiceProvider(512);

    public static string EncryptLobbyLink(string link)
    {
        var token = _rsa.Encrypt(Encoding.UTF8.GetBytes(link), RSAEncryptionPadding.Pkcs1);
        return Convert.ToBase64String(token);
    }

    public static string DecryptLobbyLink(string token)
    {
        var link = _rsa.Decrypt(Convert.FromBase64String(token), RSAEncryptionPadding.Pkcs1);
        return Encoding.UTF8.GetString(link);
    }
}