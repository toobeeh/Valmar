namespace tobeh.Valmar.Server.Domain.Classes;

public record ScopeRequestDdo(
    int TypoId,
    string ApplicationName,
    List<string> Scopes,
    DateTimeOffset Expiry,
    string RedirectUri)
{
    public override string ToString()
    {
        return $"{TypoId}:{ApplicationName}:{string.Join(",", Scopes)}:{Expiry.ToUnixTimeMilliseconds()}:{RedirectUri}";
    }
};