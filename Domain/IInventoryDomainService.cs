using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IInventoryDomainService
{
    Task<List<MemberSpriteSlotDdo>> GetMemberSpriteInventory(int login);
    Task<BubbleCreditDdo> GetBubbleCredit(int login, DropCreditDdo dropCredit);
    Task BuySprite(int login, int spriteId);
    Task<List<EventCreditDdo>> GetEventCredit(int login, List<int> eventDropIds);
    Task<DropCreditDdo> GetDropCredit(int login);
    Task<int> GetSpriteSlotCount(int login);
    Task UseSpriteCombo(int login, List<int> combo, bool clearOther = false);
}