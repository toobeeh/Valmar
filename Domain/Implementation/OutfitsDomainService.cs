using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Exceptions;
using Valmar.Util;

namespace Valmar.Domain.Implementation;

public class OutfitsDomainService(
    ILogger<OutfitsDomainService> logger, 
    IInventoryDomainService inventoryDomainService,
    PalantirContext db) : IOutfitsDomainService
{
    public async Task<List<OutfitDdo>> GetMemberOutfits(int login)
    {
        logger.LogTrace("GetMemberOutfits(login={login})", login);

        var outfits = await db.SpriteProfiles
            .Where(outfit => outfit.Login == login)
            .ToListAsync();

        return outfits
            .Select(outfit => new OutfitDdo(
                outfit.Name,
                InventoryHelper.ParseActiveSlotsFromInventory(
                    string.Join(",", outfit.Combo.Split(",").Select((item, idx) => $"{".".Repeat(idx + 1)}{item}")), 
                    outfit.RainbowSprites),
                string.IsNullOrWhiteSpace(outfit.Scene) ? null : Convert.ToInt32(outfit.Scene)))
            .ToList();
    }

    public async Task<OutfitDdo> GetMemberOutfit(int login, string name)
    {
        logger.LogTrace("GetMemberOutfit(login={login}, name={name})", login, name);
        
        var outfit = await db.SpriteProfiles
            .Where(profile => profile.Login == login && profile.Name == name)
            .FirstOrDefaultAsync();

        if (outfit == null)
        {
            throw new EntityNotFoundException($"Outfit with name {name} not found for login {login}");
        }
        
        return new OutfitDdo(
            outfit.Name,
            InventoryHelper.ParseActiveSlotsFromInventory(
                string.Join(",", outfit.Combo.Split(",").Select((item, idx) => $"{".".Repeat(idx + 1)}{item}")), 
                outfit.RainbowSprites),
            string.IsNullOrWhiteSpace(outfit.Scene) ? null : Convert.ToInt32(outfit.Scene));
    }
    
    public async Task SaveOutfit(int login, OutfitDdo outfit)
    {
        logger.LogTrace("SaveOutfit(login={login}, outfit={outfit})", login, outfit);

        var spriteProfile = await db.SpriteProfiles
            .Where(profile => profile.Login == login && profile.Name == outfit.Name)
            .FirstOrDefaultAsync();

        if (spriteProfile == null)
        {
            spriteProfile = new SpriteProfileEntity
            {
                Login = login,
                Name = outfit.Name,
                Combo = InventoryHelper.SerializeSimpleCombo(outfit.SpriteSlotConfiguration),
                RainbowSprites = InventoryHelper.SerializeSimpleColorConfig(outfit.SpriteSlotConfiguration),
                Scene = outfit.SceneId?.ToString() ?? ""
            };

            db.SpriteProfiles.Add(spriteProfile);
        }
        else
        {
            spriteProfile.Combo = InventoryHelper.SerializeSimpleCombo(outfit.SpriteSlotConfiguration);
            spriteProfile.RainbowSprites = InventoryHelper.SerializeSimpleColorConfig(outfit.SpriteSlotConfiguration);
            spriteProfile.Scene = outfit.SceneId?.ToString() ?? "";
            db.Update(spriteProfile);
        }

        await db.SaveChangesAsync();
    }

    public async Task DeleteOutfit(int login, string name)
    {
        logger.LogTrace("DeleteOutfit(login={login}, name={name})", login, name);
        
        var spriteProfile = await db.SpriteProfiles
            .Where(profile => profile.Login == login && profile.Name == name)
            .FirstOrDefaultAsync();

        if (spriteProfile == null)
        {
            throw new EntityNotFoundException($"Outfit with name {name} not found for login {login}");
        }
        
        db.SpriteProfiles.Remove(spriteProfile);
        await db.SaveChangesAsync();
    }
    
    public async Task UseOutfit(int login, string name)
    {
        logger.LogTrace("UseOutfit(login={login}, name={name})", login, name);

        var outfit = await GetMemberOutfit(login, name);

        await inventoryDomainService.UseSpriteCombo(login, outfit.SpriteSlotConfiguration.Select(s => s.SpriteId).ToList(),
            true);

        var shifts = outfit.SpriteSlotConfiguration.Where(slot => slot.ColorShift is not null)
            .ToDictionary(slot => slot.SpriteId, slot => slot.ColorShift);
        await inventoryDomainService.SetColorShiftConfiguration(login, shifts, true);

        await inventoryDomainService.UseScene(login, outfit.SceneId);
    }
}