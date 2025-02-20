using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class InventoryMapperProfile : Profile
{
    public InventoryMapperProfile()
    {
        CreateMap<MemberSpriteSlotDdo, SpriteSlotConfigurationReply>().ReverseMap();
        CreateMap<BubbleCreditDdo, BubbleCreditReply>();
        CreateMap<DropCreditDdo, DropCreditReply>();
        CreateMap<EventCreditDdo, EventCreditReply>();
        CreateMap<SceneInventoryItemDdo, SceneInventoryItemReply>();
        CreateMap<SceneInventoryDdo, SceneInventoryReply>();
        CreateMap<AwardInventoryDdo, AwardInventoryMessage>();

        CreateMap<AwardeeEntity, AvailableAwardMessage>()
            .ForMember(message => message.InventoryId, options => options.MapFrom(entity => entity.Id))
            .ForMember(message => message.AwardId, options => options.MapFrom(entity => entity.Award));
        CreateMap<AwardeeEntity, ConsumedAwardMessage>().ConvertUsing(entity => MapConsumedAward(entity));

        CreateMap<GalleryItemDdo, GalleryItemMessage>();

        CreateMap<GiftLossRateDdo, GiftLossRateMessage>();
        CreateMap<EventCreditGiftResultDdo, GiftLossMessage>();
        CreateMap<EventResultDdo, EventProgressMessage>();
        CreateMap<KeyValuePair<int, double>, EventDropProgressMessage>()
            .ForMember(msg => msg.EventDropId, options => options.MapFrom(ddo => ddo.Key))
            .ForMember(msg => msg.Value, options => options.MapFrom(ddo => ddo.Value));
    }

    private static ConsumedAwardMessage MapConsumedAward(AwardeeEntity entity)
    {
        return new ConsumedAwardMessage
        {
            AwardId = entity.Award,
            OwnerLogin = entity.OwnerLogin,
            AwardeeLogin = entity.AwardeeLogin ?? throw new NullReferenceException("Award has not been consumed"),
            LinkedImageId = entity.ImageId,
            AwardedTimestamp = Timestamp.FromDateTimeOffset(
                DateTimeOffset.FromUnixTimeMilliseconds(entity.Date ??
                                                        throw new NullReferenceException(
                                                            "Award has not been consumed")))
        };
    }
}