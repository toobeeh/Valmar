using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Domain.Classes;

public record AwardInventoryDdo(List<AwardeeEntity> AvailableAwards, List<AwardeeEntity> GivenAwards, List<AwardeeEntity> ReceivedAwards);