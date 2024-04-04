namespace Valmar.Domain.Classes;

public record SplitRewardDdo(int RewardeeLogin, SplitDefinitionDdo Split, DateTimeOffset RewardDate, string? Comment, int? ValueOverride, bool Expired);