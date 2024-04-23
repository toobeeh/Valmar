using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class OutfitsGrpcService(
    ILogger<OutfitsGrpcService> logger, 
    IMapper mapper,
    IOutfitsDomainService outfitsDomainService) : Outfits.OutfitsBase
{
    public override async Task GetOutfits(GetOutfitsRequest request, IServerStreamWriter<OutfitMessage> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetOutfits(request={request})", request);

        var outfits = await outfitsDomainService.GetMemberOutfits(request.Login);
        await responseStream.WriteAllMappedAsync(outfits, mapper.Map<OutfitMessage>);
    }

    public override async Task<OutfitMessage> GetOutfit(GetOutfitRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetOutfit(request={request})", request);

        var outfit = await outfitsDomainService.GetMemberOutfit(request.Login, request.OutfitName);
        return mapper.Map<OutfitMessage>(outfit);
    }

    public override async Task<Empty> SaveOutfit(SaveOutfitRequest request, ServerCallContext context)
    {
        logger.LogTrace("SaveOutfit(request={request})", request);
        
        await outfitsDomainService.SaveOutfit(request.Login, mapper.Map<OutfitDdo>(request.Outfit));
        return new Empty();
    }

    public override async Task<Empty> DeleteOutfit(DeleteOutfitRequest request, ServerCallContext context)
    {
        logger.LogTrace("DeleteOutfit(request={request})", request);
        
        await outfitsDomainService.DeleteOutfit(request.Login, request.OutfitName);
        return new Empty();
    }

    public override async Task<Empty> UseOutfit(UseOutfitRequest request, ServerCallContext context)
    {
        logger.LogTrace("UseOutfit(request={request})", request);
        
        await outfitsDomainService.UseOutfit(request.Login, request.OutfitName);
        return new Empty();
    }
}