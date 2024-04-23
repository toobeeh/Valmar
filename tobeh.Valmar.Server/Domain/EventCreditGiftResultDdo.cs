using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public record EventCreditGiftResultDdo(GiftLossRateDdo LossRate, int LostAmount, int TotalAmount);