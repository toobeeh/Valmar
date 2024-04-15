using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;

namespace Valmar.Mappers;

public class InventoryMapperProfile : Profile
{
    public InventoryMapperProfile()
    {
        CreateMap<MemberSpriteSlotDdo, SpriteSlotConfigurationReply>().ReverseMap();
        CreateMap<BubbleCreditDdo, BubbleCreditReply>();
        CreateMap<EventCreditDdo, EventCreditReply>();
        CreateMap<SceneInventoryDdo, SceneInventoryReply>();
    }
}