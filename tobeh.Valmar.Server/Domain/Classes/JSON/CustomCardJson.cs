namespace tobeh.Valmar.Server.Domain.Classes.JSON;

public record CustomCardJson(
    double HeaderOpacity, 
    double BackgroundOpacity, 
    string? BackgroundImage, 
    string LightTextColor, 
    string DarkTextColor, 
    string HeaderColor, 
    string? TemplateName);