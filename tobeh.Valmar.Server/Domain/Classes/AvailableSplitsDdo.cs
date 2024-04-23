namespace tobeh.Valmar.Server.Domain.Classes;

public record AvailableSplitsDdo(int TotalSplits, int AvailableSplits, List<DropboostDdo> ActiveDropboosts, bool CanStartBoost);