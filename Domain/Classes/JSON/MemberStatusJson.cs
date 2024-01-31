using System.Text.Json.Serialization;

namespace Valmar.Domain.Classes.JSON;

public record MemberStatusJson(
    string Status,
    [property: JsonPropertyName("LobbyID")] string LobbyId,
    [property: JsonPropertyName("LobbyPlayerID")] string LobbyPlayerId,
    MemberStatusDetailsJson PlayerMember
    );
    
public record MemberStatusDetailsJson(
    string UserName, 
    string UserLogin
    );