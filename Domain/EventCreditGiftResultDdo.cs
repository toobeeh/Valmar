using Valmar.Domain.Classes;

namespace Valmar.Domain;

public record EventCreditGiftResultDdo(GiftLossRateDdo LossRate, int LostAmount, int TotalAmount);