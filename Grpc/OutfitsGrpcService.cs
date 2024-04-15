using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Domain.Classes;
using Valmar.Grpc.Utils;

namespace Valmar.Grpc;

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