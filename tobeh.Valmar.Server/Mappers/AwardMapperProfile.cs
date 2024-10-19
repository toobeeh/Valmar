using AutoMapper;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class AwardMapperProfile : Profile
{
    public AwardMapperProfile()
    {
        CreateMap<int, AwardRarityMessage>().ConstructUsing(rarity => MapAwardRarity(rarity));
        CreateMap<sbyte, AwardRarityMessage>().ConstructUsing(rarity => MapAwardRarity(rarity));
        CreateMap<AwardEntity, AwardReply>();
    }

    private static AwardRarityMessage MapAwardRarity(int rarity)
    {
        return rarity switch
        {
            1 => AwardRarityMessage.Common,
            2 => AwardRarityMessage.Special,
            3 => AwardRarityMessage.Epic,
            4 => AwardRarityMessage.Legendary,
            _ => throw new ArgumentException($"Rarity value {rarity} is not valid.")
        };
    }

    private static int MapAwardRarity(AwardRarityMessage rarity)
    {
        return (int)rarity + 1;
    }
}