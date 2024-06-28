using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class OutfitMapperProfile : Profile
{
    public OutfitMapperProfile()
    {
        CreateMap<OutfitDdo, OutfitMessage>()
            .ConstructUsing(source => ConstructFromDdo(source))
            .ReverseMap()
            .ConstructUsing(source => ConstructFromMessage(source));
    }

    private static OutfitMessage ConstructFromDdo(OutfitDdo source)
    {
        return new OutfitMessage
        {
            Name = source.Name,
            SpriteSlotConfiguration =
            {
                source.SpriteSlotConfiguration.Select(slot => new SpriteSlotConfigurationReply
                {
                    Slot = slot.Slot,
                    SpriteId = slot.SpriteId,
                    ColorShift = slot.ColorShift
                })
            },
            SceneId = source.Scene?.SceneId,
            SceneShift = source.Scene?.SceneShift
        };
    }

    private static OutfitDdo ConstructFromMessage(OutfitMessage source)
    {
        return new OutfitDdo(source.Name,
            source.SpriteSlotConfiguration.Select(slot => new MemberSpriteSlotDdo(
                slot.Slot, slot.SpriteId, slot.ColorShift)).ToList(),
            source.SceneId is { } sceneValue ? new SceneInventoryItemDdo(sceneValue, source.SceneShift) : null
        );
    }
}