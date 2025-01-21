using System.Security.Cryptography;
using System.Text;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Util;

public static class LobbiesHelper
{
    private static readonly RSA Rsa = new RSACryptoServiceProvider(512);

    public static string EncryptLobbyLink(PlainLobbyLinkDdo link)
    {
        var token = Rsa.Encrypt(Encoding.UTF8.GetBytes($"{link.GuildId}:{link.Link}"), RSAEncryptionPadding.Pkcs1);
        return Convert.ToBase64String(token);
    }

    public static PlainLobbyLinkDdo DecryptLobbyLink(string token)
    {
        var content = Rsa.Decrypt(Convert.FromBase64String(token), RSAEncryptionPadding.Pkcs1);
        var stringContent = Encoding.UTF8.GetString(content);
        var guild = stringContent.Split(":")[0];
        var link = stringContent[(guild.Length + 1)..];
        return new PlainLobbyLinkDdo(link, Convert.ToInt64(guild));
    }

    /// <summary>
    /// Generate a lobby key from a lobby id
    /// this has the purpose of comparing lobbies without revealing the id
    /// </summary>
    /// <param name="lobbyId"></param>
    /// <returns></returns>
    public static string CalculateLobbyKey(string lobbyId)
    {
        var sum = lobbyId.Sum(c => c);
        var hashed = lobbyId.Select(c => (char)(c + sum));
        var key = new string(hashed.ToArray());
        return key;
    }
}