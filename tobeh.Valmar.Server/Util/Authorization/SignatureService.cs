using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace tobeh.Valmar.Server.Util.Authorization;

public class SignatureService
{
    private readonly RSA _privateRsa;
    private readonly RSA _publicRsa;

    public SignatureService(IOptions<AuthorizationConfig> options)
    {
        if (options.Value == null)
            throw new ArgumentNullException(nameof(options));

        _privateRsa = LoadPrivateKey(options.Value.PrivateKeyLocation);
        _publicRsa = LoadPublicKey(options.Value.PublicKeyLocation);
        SigningCredentials = new SigningCredentials(
            new RsaSecurityKey(_privateRsa),
            SecurityAlgorithms.RsaSha256
        );
        SigningCredentials.Key.KeyId = "default";
    }

    public string Sign(string data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signature = _privateRsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    public bool Verify(string data, string base64Signature)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signatureBytes = Convert.FromBase64String(base64Signature);
        return _publicRsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    public SigningCredentials SigningCredentials { get; }

    private static RSA LoadPrivateKey(string path)
    {
        var pem = File.ReadAllText(path);
        var rsa = RSA.Create();
        rsa.ImportFromPem(pem.ToCharArray());
        return rsa;
    }

    private static RSA LoadPublicKey(string path)
    {
        var pem = File.ReadAllText(path);
        var rsa = RSA.Create();
        rsa.ImportFromPem(pem.ToCharArray());
        return rsa;
    }

    public string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var randomBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var result = new StringBuilder(length);
        foreach (var b in randomBytes)
        {
            result.Append(chars[b % chars.Length]);
        }

        return result.ToString();
    }
}