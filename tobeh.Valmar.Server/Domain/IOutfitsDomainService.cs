using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Domain;

public interface IOutfitsDomainService
{
    Task<List<OutfitDdo>> GetMemberOutfits(int login);
    Task SaveOutfit(int login, OutfitDdo outfit);
    Task DeleteOutfit(int login, string name);
    Task<OutfitDdo> GetMemberOutfit(int login, string name);
    Task UseOutfit(int login, string name);
}