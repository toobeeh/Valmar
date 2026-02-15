using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;

namespace tobeh.Valmar.Server.Grpc;

public class GuildsGrpcService(
    ILogger<GuildsGrpcService> logger,
    IMapper mapper,
    IGuildsDomainService guildsService,
    IMembersDomainService membersDomainService
) : Guilds.GuildsBase
{
    public override async Task<GuildReply> GetGuildByInvite(GetGuildRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetGuildByToken(request={request})", request);

        var details = await guildsService.GetGuildByInvite(request.Invite);
        return mapper.Map<GuildReply>(details);
    }

    public override async Task<GuildReply> GetGuildById(GetGuildByIdMessage request, ServerCallContext context)
    {
        logger.LogTrace("GetGuildByDiscordId(request={request})", request);

        var details = await guildsService.GetGuildByDiscordId(request.DiscordId);
        return mapper.Map<GuildReply>(details);
    }

    public override async Task<GuildReply> GetValidGuilds(Empty request, ServerCallContext context)
    {
        logger.LogTrace("GetValidGuilds()");

        var details = await guildsService.GetValidGuilds();
        return mapper.Map<GuildReply>(details);
    }

    public override async Task GetGuildSupporters(GetGuildSupportersMessage request,
        IServerStreamWriter<MemberReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetGuildSupporters(request={request})", request);

        var supporters = await guildsService.GetGuildSupporters(request.GuildId);
        foreach (var supporter in supporters)
        {
            var member = await membersDomainService.GetMemberByLogin(supporter);
            await responseStream.WriteAsync(mapper.Map<MemberReply>(member));
        }
    }

    public override async Task<GuildOptionsMessage> GetGuildOptionsById(GetGuildOptionsByIdMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("GetGuildOptionsById(request={request})", request);

        var options = await guildsService.GetGuildOptionsByGuildId(request.GuildId);
        return mapper.Map<GuildOptionsMessage>(options);
    }

    public override async Task<Empty> SetGuildOptions(GuildOptionsMessage request, ServerCallContext context)
    {
        logger.LogTrace("SetGuildOptions(request={request})", request);

        await guildsService.UpdateGuildOptions(request.GuildId, request.Name, request.Prefix, request.ShowInvite,
            request.ProxyLinks, request.ChannelId,
            request.BotName, request.AnnouncementsWebhook);
        return new Empty();
    }

    public override async Task<Empty> AddGuildWebhook(AddGuildWebhookMessage request, ServerCallContext context)
    {
        logger.LogTrace("AddGuildWebhook(request={request})", request);

        await guildsService.AddGuildWebhook(request.GuildId, request.Url, request.Name);
        return new Empty();
    }

    public override async Task<Empty> RemoveGuildWebhook(RemoveGuildWebhookMessage request, ServerCallContext context)
    {
        logger.LogTrace("RemoveGuildWebhook(request={request})", request);

        await guildsService.RemoveGuildWebhook(request.GuildId, request.Name);
        return new Empty();
    }

    public override async Task GetGuildWebhooks(GetGuildWebhooksMessage request,
        IServerStreamWriter<GuildWebhookMessage> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetGuildWebhooks(request={request})", request);

        var webhooks = await guildsService.GetGuildWebhooks(request.GuildId);
        foreach (var webhook in webhooks)
        {
            await responseStream.WriteAsync(mapper.Map<GuildWebhookMessage>(webhook));
        }
    }

    public override async Task GetGuildBans(GetGuildBansMessage request,
        IServerStreamWriter<MemberReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetGuildBans(request={request})", request);

        var bans = await guildsService.GetBannedMembers(request.GuildId);
        foreach (var ban in bans)
        {
            var member = await membersDomainService.GetMemberByLogin(ban);
            await responseStream.WriteAsync(mapper.Map<MemberReply>(member));
        }
    }

    public override async Task<Empty> BanGuildMember(BanGuildMemberMessage request, ServerCallContext context)
    {
        logger.LogTrace("BanGuildMember(request={request})", request);

        var member = await membersDomainService.GetMemberByDiscordId(request.MemberId);

        if (request.Ban)
        {
            await guildsService.BanMemberFromGuild(member.Login, request.GuildId);
        }
        else
        {
            await guildsService.UnbanMemberFromGuild(member.Login, request.GuildId);
        }

        return new Empty();
    }
}