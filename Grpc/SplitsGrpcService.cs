using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Grpc.Utils;
using Valmar.Util;

namespace Valmar.Grpc;

public class SplitsGrpcService(
    ILogger<SplitsGrpcService> logger, 
    IMapper mapper,
    IMembersDomainService membersDomainService,
    ISplitsDomainService splitsDomainService) : Splits.SplitsBase
{
    public override async Task GetSplits(Empty request, IServerStreamWriter<SplitReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetSplits(request={request})", request);
        
        var splits = await splitsDomainService.GetSplits();
        await responseStream.WriteAllMappedAsync(splits, mapper.Map<SplitReply>);
    }

    public override async Task<SplitReply> GetSplitById(GetSplitByIdRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetSplitById(request={request})", request);
        
        var split = await splitsDomainService.GetSplitById(request.Id);
        return mapper.Map<SplitReply>(split);
    }

    public override async Task GetMemberSplitRewards(GetMemberSplitRewardsRequest request, IServerStreamWriter<SplitRewardReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetMemberSplitRewards(request={request})", request);

        var member = await membersDomainService.GetMemberByLogin(request.Login);
        var rewards = await splitsDomainService.GetMemberSplitRewards(member);
        await responseStream.WriteAllMappedAsync(rewards, mapper.Map<SplitRewardReply>);
    }

    public override async Task<Empty> RewardSplit(RewardSplitRequest request, ServerCallContext context)
    {
        logger.LogTrace("RewardSplit(request={request})", request);
        
        await splitsDomainService.RewardSplit(request.RewardeeLogin, request.SplitId, request.Comment, request.ValueOverride);
        return new Empty();
    }

    public override async Task<BoostCostInformationReply> GetBoostCostInformation(Empty request, ServerCallContext context)
    {
        logger.LogTrace("GetBoostCostInformation(request={request})", request);

        return new BoostCostInformationReply
        {
            FactorSplitCost = SplitHelper.FactorSplitCost,
            FactorIncrease = SplitHelper.FactorIncrease,
            DurationSplitCost = SplitHelper.DurationSplitCost,
            DurationIncreaseMinutes = SplitHelper.DurationIncreaseMinutes,
            CooldownSplitCost = SplitHelper.CooldownSplitCost,
            CooldownIncreaseHours = SplitHelper.CooldownIncreaseHours,
            DefaultFactor = SplitHelper.DefaultFactor,
            DefaultDurationMinutes = SplitHelper.DefaultDurationMinutes,
            DefaultCooldownHours = SplitHelper.DefaultCooldownHours
        };
    }

    public override async Task GetActiveDropboosts(Empty request, IServerStreamWriter<ActiveDropboostReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetActiveDropboosts(request={request})", request);
        
        var boosts = await splitsDomainService.GetDropboosts();
        await responseStream.WriteAllMappedAsync(boosts, mapper.Map<ActiveDropboostReply>);
    }

    public override async Task GetActiveDropboostsOfMember(GetActiveDropboostsForMemberRequest request, IServerStreamWriter<ActiveDropboostReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetActiveDropboostsOfMember(request={request})", request);
        
        var boosts = await splitsDomainService.GetDropboosts(request.Login);
        await responseStream.WriteAllMappedAsync(boosts, mapper.Map<ActiveDropboostReply>);
    }

    public override async Task<Empty> StartDropboost(StartDropboostRequest request, ServerCallContext context)
    {
        logger.LogTrace("StartDropboost(request={request})", request);

        var member = await membersDomainService.GetMemberByLogin(request.Login);
        await splitsDomainService.StartDropboost(member, request.FactorSplits, request.DurationSplits,
            request.CooldownSplits);

        return new Empty();
    }

    public override async Task<AvailableSplitsReply> GetAvailableSplits(GetAvailableSplitsRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetAvailableSplits(request={request})", request);

        var member = await membersDomainService.GetMemberByLogin(request.Login);
        var available = await splitsDomainService.GetAvailableSplits(member);
        return mapper.Map<AvailableSplitsReply>(available);
    }
}