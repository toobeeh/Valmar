using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IInventoryDomainService
{
    Task<List<MemberSpriteSlotDdo>> GetMemberSpriteInventory(int login);
    Task<BubbleCreditDdo> GetBubbleCredit(int login, DropCreditDdo dropCredit);
    Task BuySprite(MemberDdo member, int spriteId);
    Task<List<EventCreditDdo>> GetEventCredit(MemberDdo member, List<int> eventDropIds);
    Task<DropCreditDdo> GetDropCredit(int login);
    Task<int> GetSpriteSlotCount(int login);
    Task UseSpriteCombo(int login, List<int> combo, bool clearOther = false);
    Task SetColorShiftConfiguration(int login, Dictionary<int, int?> colorShiftMap, bool clearOther = false);
    Task<SceneInventoryDdo> GetMemberSceneInventory(int login);
    Task UseScene(int login, int? sceneId);
    Task BuyScene(int login, int sceneId);
    Task<AwardInventoryDdo> GetMemberAwardInventory(int login);
    Task<List<GalleryItemDdo>> GetImagesFromCloud(MemberDdo member, List<long> ids);
    Task<List<AwardEntity>> OpenAwardPack(MemberDdo member, int rarityLevel);
    Task<DateTimeOffset> GetFirstSeenDate(int login);
    Task<GiftLossRateDdo> GetGiftLossRateBase(MemberDdo member, List<SpriteDdo> eventSprites);
    Task<EventCreditGiftResultDdo> GiftEventCredit(MemberDdo fromMember, MemberDdo toMember, int amount, EventDropDdo eventDrop, GiftLossRateDdo lossRate);
    Task<int> RedeemEventLeagueDrops(MemberDdo member, int amount, int eventDropId);
}