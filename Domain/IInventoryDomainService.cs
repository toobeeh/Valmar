using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IInventoryDomainService
{
    Task<List<MemberSpriteSlotDdo>> GetMemberSpriteInventory(int login);
    Task<BubbleCreditDdo> GetBubbleCredit(int login);
    Task BuySprite(int login, int spriteId);
    Task<List<EventCreditDdo>> GetEventCredit(int login, List<int> eventDropIds);
}