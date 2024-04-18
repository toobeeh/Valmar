using Valmar.Database;

namespace Valmar.Domain.Classes;

public record AwardInventoryDdo(List<AwardeeEntity> AvailableAwards, List<AwardeeEntity> GivenAwards, List<AwardeeEntity> ReceivedAwards);