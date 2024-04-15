using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IOutfitsDomainService
{
    Task<List<OutfitDdo>> GetMemberOutfits(int login);
    Task SaveOutfit(int login, OutfitDdo outfit);
    Task DeleteOutfit(int login, string name);
    Task<OutfitDdo> GetMemberOutfit(int login, string name);
    Task UseOutfit(int login, string name);
}