namespace tobeh.Valmar.Server.Domain.Classes;

public record OAuth2AuthorizationCodeDdo(string OAuth2AuthorizationCode, int OAuth2ClientId, string RedirectUri);