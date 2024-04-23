using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes.JSON;
using tobeh.Valmar.Server.Domain.Implementation;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class CardGrpcService(
    ILogger<CardGrpcService> logger, 
    IMapper mapper,
    IMembersDomainService membersDomainService,
    ICardDomainService cardDomainService) : Card.CardBase
{
    public override async Task GetCardTemplates(Empty request, IServerStreamWriter<CardTemplateListingMessage> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetCardTemplates(request={request})", request);
        
        var cardTemplates = await cardDomainService.GetAllTemplates();
        await responseStream.WriteAllMappedAsync(cardTemplates, mapper.Map<CardTemplateListingMessage>);
    }

    public override async Task<CardTemplateMessage> GetCardTemplate(GetCardTemplateMessage request, ServerCallContext context)
    {
        logger.LogTrace("GetCardTemplate(request={request})", request);
        
        var cardTemplate = await cardDomainService.GetTemplate(request.Name);
        return mapper.Map<CardTemplateMessage>(cardTemplate);
    }

    public override async Task<MemberCardSettingsMessage> GetMemberCardSettings(GetMemberCardSettingsMessage request, ServerCallContext context)
    {
        logger.LogTrace("GetMemberCardSettings(request={request})", request);

        var member = await membersDomainService.GetMemberByLogin(request.Login);
        var cardSettings = await cardDomainService.GetCustomCardSettings(member);

        return mapper.Map<MemberCardSettingsMessage>(cardSettings);
    }

    public override async Task<Empty> SetMemberCardSettings(SetMemberCardSettingsMessage request, ServerCallContext context)
    {
        logger.LogTrace("SetMemberCardSettings(request={request})", request);
        
        var member = await membersDomainService.GetMemberByLogin(request.Login);
        var cardSettings = mapper.Map<CustomCardJson>(request);
        await cardDomainService.SaveCustomCardSettings(member, cardSettings);
        
        return new Empty();
    }
}