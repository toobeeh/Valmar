using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IInventoryDomainService
{
    Task<List<MemberSpriteSlotDdo>> GetMemberSpriteInventory(MemberDdo member);
    Task<BubbleCreditDdo> GetBubbleCredit(MemberDdo member);
    Task BuySprite(MemberDdo member, int spriteId);
    Task<List<EventCreditDdo>> GetEventCredit(MemberDdo member, List<int> eventDropIds);
    Task<DropCreditDdo> GetDropCredit(MemberDdo member);
    Task<int> GetSpriteSlotCount(MemberDdo member);
    Task UseSpriteCombo(MemberDdo member, List<int?> combo, bool clearOther = false);
    Task SetColorShiftConfiguration(MemberDdo member, Dictionary<int, int?> colorShiftMap, bool clearOther = false);
    Task<SceneInventoryDdo> GetMemberSceneInventory(MemberDdo member);
    Task UseScene(MemberDdo member, int? sceneId, int? sceneShift);
    Task BuyScene(MemberDdo member, int sceneId, int? sceneShift);
    Task<AwardInventoryDdo> GetMemberAwardInventory(int login);
    Task<List<GalleryItemDdo>> GetImagesFromCloud(MemberDdo member, List<long> ids);
    Task<List<AwardEntity>> OpenAwardPack(MemberDdo member, int rarityLevel);
    Task<DateTimeOffset> GetFirstSeenDate(int login);
    Task<GiftLossRateDdo> GetGiftLossRateBase(MemberDdo member, List<SpriteDdo> eventSprites);

    Task<EventCreditGiftResultDdo> GiftEventCredit(MemberDdo fromMember, MemberDdo toMember, int amount,
        EventDropDdo eventDrop, GiftLossRateDdo lossRate);

    Task SetPatronEmoji(MemberDdo member, string? emoji);
    Task SetPatronizedMember(MemberDdo member, long? patronizedMemberDiscordId);
    Task<EventResultDdo> GetEventProgress(MemberDdo member);
    Task<double> GetNextSlotRemainingDrops(MemberDdo member);
    Task GiveAward(int login, int awardInventoryId, string lobbyId, int receiverLobbyPlayerId);
}