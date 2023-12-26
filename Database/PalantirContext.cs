using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Valmar.Database
{
    public partial class PalantirContext : DbContext
    {
        public virtual DbSet<AccessTokenEntity> AccessTokens { get; set; } = null!;
        public virtual DbSet<AwardEntity> Awards { get; set; } = null!;
        public virtual DbSet<AwardeeEntity> Awardees { get; set; } = null!;
        public virtual DbSet<BoostSplitEntity> BoostSplits { get; set; } = null!;
        public virtual DbSet<BubbleTraceEntity> BubbleTraces { get; set; } = null!;
        public virtual DbSet<CloudTagEntity> CloudTags { get; set; } = null!;
        public virtual DbSet<DropBoostEntity> DropBoosts { get; set; } = null!;
        public virtual DbSet<EventEntity> Events { get; set; } = null!;
        public virtual DbSet<EventCreditEntity> EventCredits { get; set; } = null!;
        public virtual DbSet<EventDropEntity> EventDrops { get; set; } = null!;
        public virtual DbSet<GuildLobbyEntity> GuildLobbies { get; set; } = null!;
        public virtual DbSet<GuildSettingEntity> GuildSettings { get; set; } = null!;
        public virtual DbSet<LobEntity> Lobs { get; set; } = null!;
        public virtual DbSet<LobbyEntity> Lobbies { get; set; } = null!;
        public virtual DbSet<MemberEntity> Members { get; set; } = null!;
        public virtual DbSet<NextDropEntity> NextDrops { get; set; } = null!;
        public virtual DbSet<OnlineItemEntity> OnlineItems { get; set; } = null!;
        public virtual DbSet<OnlineSpriteEntity> OnlineSprites { get; set; } = null!;
        public virtual DbSet<PalantiriEntity> Palantiris { get; set; } = null!;
        public virtual DbSet<PalantiriNightlyEntity> PalantiriNightlies { get; set; } = null!;
        public virtual DbSet<PastDropEntity> PastDrops { get; set; } = null!;
        public virtual DbSet<ReportEntity> Reports { get; set; } = null!;
        public virtual DbSet<SceneEntity> Scenes { get; set; } = null!;
        public virtual DbSet<SpEntity> Sps { get; set; } = null!;
        public virtual DbSet<SplitCreditEntity> SplitCredits { get; set; } = null!;
        public virtual DbSet<SpriteEntity> Sprites { get; set; } = null!;
        public virtual DbSet<SpriteProfileEntity> SpriteProfiles { get; set; } = null!;
        public virtual DbSet<StatusEntity> Statuses { get; set; } = null!;
        public virtual DbSet<ThemeEntity> Themes { get; set; } = null!;
        public virtual DbSet<ThemeShareEntity> ThemeShares { get; set; } = null!;
        public virtual DbSet<UserThemeEntity> UserThemes { get; set; } = null!;
        public virtual DbSet<WebhookEntity> Webhooks { get; set; } = null!;

        public PalantirContext()
        {
        }

        public PalantirContext(DbContextOptions<PalantirContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("name=ConnectionStrings:Palantir", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.3-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<AccessTokenEntity>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.ToTable("AccessTokens");

                entity.Property(e => e.Login)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccessToken1)
                    .HasColumnType("text")
                    .HasColumnName("AccessToken");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("current_timestamp()");
            });

            modelBuilder.Entity<AwardEntity>(entity =>
            {
                entity.ToTable("Awards");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.Rarity).HasColumnType("tinyint(4)");

                entity.Property(e => e.Url)
                    .HasColumnType("text")
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<AwardeeEntity>(entity =>
            {
                entity.ToTable("Awardees");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Award).HasColumnType("smallint(6)");

                entity.Property(e => e.AwardeeLogin).HasColumnType("int(6)");

                entity.Property(e => e.Date).HasColumnType("bigint(20)");

                entity.Property(e => e.ImageId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("ImageID");

                entity.Property(e => e.OwnerLogin).HasColumnType("int(6)");
            });

            modelBuilder.Entity<BoostSplitEntity>(entity =>
            {
                entity.ToTable("BoostSplits");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("text");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.Value).HasColumnType("int(11)");
            });

            modelBuilder.Entity<BubbleTraceEntity>(entity =>
            {
                entity.ToTable("BubbleTraces");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Bubbles).HasColumnType("int(11)");

                entity.Property(e => e.Date).HasColumnType("text");

                entity.Property(e => e.Login).HasColumnType("int(11)");
            });

            modelBuilder.Entity<CloudTagEntity>(entity =>
            {
                entity.HasKey(e => new { e.Owner, e.ImageId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("CloudTags");

                entity.Property(e => e.Owner).HasColumnType("int(11)");

                entity.Property(e => e.ImageId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("ImageID");

                entity.Property(e => e.Author).HasMaxLength(14);

                entity.Property(e => e.Date).HasColumnType("bigint(20)");

                entity.Property(e => e.Language).HasMaxLength(10);

                entity.Property(e => e.Title).HasMaxLength(30);
            });

            modelBuilder.Entity<DropBoostEntity>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.ToTable("DropBoosts");

                entity.Property(e => e.Login)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CooldownBonusS).HasColumnType("int(11)");

                entity.Property(e => e.DurationS).HasColumnType("int(11)");

                entity.Property(e => e.Factor).HasColumnType("text");

                entity.Property(e => e.StartUtcs)
                    .HasColumnType("text")
                    .HasColumnName("StartUTCS");
            });

            modelBuilder.Entity<EventEntity>(entity =>
            {
                entity.ToTable("Events");

                entity.Property(e => e.EventId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("EventID");

                entity.Property(e => e.DayLength).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.EventName).HasColumnType("text");

                entity.Property(e => e.Progressive).HasColumnType("tinyint(4)");

                entity.Property(e => e.ValidFrom).HasColumnType("text");
            });

            modelBuilder.Entity<EventCreditEntity>(entity =>
            {
                entity.HasKey(e => new { e.Login, e.EventDropId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("EventCredits");

                entity.Property(e => e.Login).HasColumnType("int(11)");

                entity.Property(e => e.EventDropId)
                    .HasColumnType("int(11)")
                    .HasColumnName("EventDropID");

                entity.Property(e => e.Credit).HasColumnType("int(11)");
            });

            modelBuilder.Entity<EventDropEntity>(entity =>
            {
                entity.ToTable("EventDrops");

                entity.Property(e => e.EventDropId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("EventDropID");

                entity.Property(e => e.EventId)
                    .HasColumnType("int(11)")
                    .HasColumnName("EventID");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.Url)
                    .HasColumnType("text")
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<GuildLobbyEntity>(entity =>
            {
                entity.HasKey(e => e.GuildId)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });

                entity.ToTable("GuildLobbies");

                entity.Property(e => e.GuildId)
                    .HasColumnType("text")
                    .HasColumnName("GuildID");

                entity.Property(e => e.Lobbies).HasColumnType("text");
            });

            modelBuilder.Entity<GuildSettingEntity>(entity =>
            {
                entity.HasKey(e => e.GuildId)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });

                entity.ToTable("GuildSettings");

                entity.Property(e => e.GuildId)
                    .HasColumnType("text")
                    .HasColumnName("GuildID");

                entity.Property(e => e.Settings).HasColumnType("text");
            });

            modelBuilder.Entity<LobEntity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("lobs");

                entity.Property(e => e.JsonUnquoteDcName)
                    .HasColumnName("JSON_UNQUOTE(DcName)")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.NameExp3)
                    .HasColumnType("mediumtext")
                    .HasColumnName("Name_exp_3")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.PlayerLobbyId)
                    .HasColumnType("mediumtext")
                    .HasColumnName("PlayerLobbyID")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");
            });

            modelBuilder.Entity<LobbyEntity>(entity =>
            {
                entity.HasKey(e => e.LobbyId)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });

                entity.ToTable("Lobbies");

                entity.Property(e => e.LobbyId)
                    .HasColumnType("text")
                    .HasColumnName("LobbyID");

                entity.Property(e => e.Lobby1)
                    .HasColumnType("text")
                    .HasColumnName("Lobby");
            });

            modelBuilder.Entity<MemberEntity>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.ToTable("Members");

                entity.Property(e => e.Login)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AwardPackOpened).HasColumnType("bigint(20)");

                entity.Property(e => e.Bubbles).HasColumnType("int(11)");

                entity.Property(e => e.Customcard).HasColumnType("text");

                entity.Property(e => e.Drops).HasColumnType("int(11)");

                entity.Property(e => e.Emoji).HasColumnType("text");

                entity.Property(e => e.Flag).HasColumnType("int(11)");

                entity.Property(e => e.Member1)
                    .HasColumnType("text")
                    .HasColumnName("Member");

                entity.Property(e => e.Patronize).HasColumnType("text");

                entity.Property(e => e.RainbowSprites).HasColumnType("text");

                entity.Property(e => e.Scenes)
                    .HasColumnType("text")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Sprites)
                    .HasColumnType("text")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Streamcode)
                    .HasColumnType("text")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<NextDropEntity>(entity =>
            {
                entity.HasKey(e => e.DropId)
                    .HasName("PRIMARY");

                entity.ToTable("NextDrop");

                entity.Property(e => e.DropId)
                    .HasColumnType("bigint(20)")
                    .ValueGeneratedNever()
                    .HasColumnName("DropID");

                entity.Property(e => e.CaughtLobbyKey).HasColumnType("text");

                entity.Property(e => e.CaughtLobbyPlayerId)
                    .HasColumnType("text")
                    .HasColumnName("CaughtLobbyPlayerID");

                entity.Property(e => e.EventDropId)
                    .HasColumnType("int(11)")
                    .HasColumnName("EventDropID");

                entity.Property(e => e.LeagueWeight).HasColumnType("int(11)");

                entity.Property(e => e.ValidFrom).HasColumnType("text");
            });

            modelBuilder.Entity<OnlineItemEntity>(entity =>
            {
                entity.HasKey(e => new { e.ItemType, e.Slot, e.LobbyKey, e.LobbyPlayerId, e.Date })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 0, 32, 0, 0 });

                entity.ToTable("OnlineItems");

                entity.Property(e => e.ItemType).HasColumnType("text");

                entity.Property(e => e.Slot).HasColumnType("int(11)");

                entity.Property(e => e.LobbyKey).HasColumnType("text");

                entity.Property(e => e.LobbyPlayerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("LobbyPlayerID");

                entity.Property(e => e.Date).HasColumnType("int(20)");

                entity.Property(e => e.ItemId)
                    .HasColumnType("bigint(11)")
                    .HasColumnName("ItemID");
            });

            modelBuilder.Entity<OnlineSpriteEntity>(entity =>
            {
                entity.HasKey(e => new { e.LobbyKey, e.LobbyPlayerId, e.Slot })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 0, 0 });

                entity.ToTable("OnlineSprites");

                entity.Property(e => e.LobbyKey).HasColumnType("text");

                entity.Property(e => e.LobbyPlayerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("LobbyPlayerID");

                entity.Property(e => e.Slot).HasColumnType("int(11)");

                entity.Property(e => e.Date).HasColumnType("text");

                entity.Property(e => e.Id)
                    .HasColumnType("text")
                    .HasColumnName("ID");

                entity.Property(e => e.Sprite).HasColumnType("int(11)");
            });

            modelBuilder.Entity<PalantiriEntity>(entity =>
            {
                entity.HasKey(e => e.Token)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });

                entity.ToTable("Palantiri");

                entity.Property(e => e.Token).HasColumnType("text");

                entity.Property(e => e.Palantir).HasColumnType("text");
            });

            modelBuilder.Entity<PalantiriNightlyEntity>(entity =>
            {
                entity.HasKey(e => e.Token)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });

                entity.ToTable("PalantiriNightly");

                entity.Property(e => e.Token).HasColumnType("text");

                entity.Property(e => e.Palantir).HasColumnType("text");
            });

            modelBuilder.Entity<PastDropEntity>(entity =>
            {
                entity.HasKey(e => new { e.DropId, e.CaughtLobbyKey, e.CaughtLobbyPlayerId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("PastDrops");

                entity.Property(e => e.DropId)
                    .HasColumnType("bigint(11)")
                    .HasColumnName("DropID");

                entity.Property(e => e.CaughtLobbyKey).HasMaxLength(50);

                entity.Property(e => e.CaughtLobbyPlayerId)
                    .HasMaxLength(20)
                    .HasColumnName("CaughtLobbyPlayerID");

                entity.Property(e => e.EventDropId)
                    .HasColumnType("int(11)")
                    .HasColumnName("EventDropID");

                entity.Property(e => e.LeagueWeight).HasColumnType("int(11)");

                entity.Property(e => e.ValidFrom).HasColumnType("text");
            });

            modelBuilder.Entity<ReportEntity>(entity =>
            {
                entity.HasKey(e => new { e.LobbyId, e.ObserveToken })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 0 });

                entity.ToTable("Reports");

                entity.Property(e => e.LobbyId)
                    .HasColumnType("text")
                    .HasColumnName("LobbyID");

                entity.Property(e => e.ObserveToken).HasColumnType("int(11)");

                entity.Property(e => e.Date).HasColumnType("text");

                entity.Property(e => e.Report1)
                    .HasColumnType("text")
                    .HasColumnName("Report");
            });

            modelBuilder.Entity<SceneEntity>(entity =>
            {
                entity.ToTable("Scenes");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Artist).HasColumnType("text");

                entity.Property(e => e.Color).HasColumnType("text");

                entity.Property(e => e.EventId)
                    .HasColumnType("int(11)")
                    .HasColumnName("EventID");

                entity.Property(e => e.GuessedColor).HasColumnType("text");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.Url)
                    .HasColumnType("text")
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<SpEntity>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("sps");

                entity.Property(e => e.Date).HasColumnType("text");

                entity.Property(e => e.DateAddCurrentTimestampInterval30Second)
                    .HasColumnType("datetime")
                    .HasColumnName("DATE_ADD(CURRENT_TIMESTAMP, INTERVAL -30 SECOND)");

                entity.Property(e => e.Id)
                    .HasColumnType("text")
                    .HasColumnName("ID");

                entity.Property(e => e.LobbyKey).HasColumnType("text");

                entity.Property(e => e.LobbyPlayerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("LobbyPlayerID");

                entity.Property(e => e.Slot).HasColumnType("int(11)");

                entity.Property(e => e.Sprite).HasColumnType("int(11)");
            });

            modelBuilder.Entity<SplitCreditEntity>(entity =>
            {
                entity.ToTable("SplitCredits");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Comment)
                    .HasColumnType("text")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Login).HasColumnType("int(11)");

                entity.Property(e => e.RewardDate).HasColumnType("text");

                entity.Property(e => e.Split).HasColumnType("int(11)");

                entity.Property(e => e.ValueOverride)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("-1");
            });

            modelBuilder.Entity<SpriteEntity>(entity =>
            {
                entity.ToTable("Sprites");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Artist)
                    .HasColumnType("text")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Cost).HasColumnType("int(11)");

                entity.Property(e => e.EventDropId)
                    .HasColumnType("int(11)")
                    .HasColumnName("EventDropID");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.Rainbow).HasColumnType("int(11)");

                entity.Property(e => e.Url)
                    .HasColumnType("text")
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<SpriteProfileEntity>(entity =>
            {
                entity.HasKey(e => new { e.Login, e.Name })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 32 });

                entity.ToTable("SpriteProfiles");

                entity.Property(e => e.Login).HasColumnType("int(11)");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.Combo).HasColumnType("text");

                entity.Property(e => e.RainbowSprites).HasColumnType("text");

                entity.Property(e => e.Scene).HasColumnType("text");
            });

            modelBuilder.Entity<StatusEntity>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });

                entity.ToTable("Status");

                entity.Property(e => e.SessionId)
                    .HasColumnType("text")
                    .HasColumnName("SessionID");

                entity.Property(e => e.Date).HasColumnType("text");

                entity.Property(e => e.Status1)
                    .HasColumnType("text")
                    .HasColumnName("Status");
            });

            modelBuilder.Entity<ThemeEntity>(entity =>
            {
                entity.HasKey(e => e.Ticket)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });

                entity.ToTable("Themes");

                entity.Property(e => e.Ticket).HasColumnType("text");

                entity.Property(e => e.Author).HasColumnType("text");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.Theme1)
                    .HasColumnType("text")
                    .HasColumnName("Theme");

                entity.Property(e => e.ThumbnailGame).HasColumnType("text");

                entity.Property(e => e.ThumbnailLanding).HasColumnType("text");
            });

            modelBuilder.Entity<ThemeShareEntity>(entity =>
            {
                entity.ToTable("ThemeShares");

                entity.Property(e => e.Id)
                    .HasMaxLength(8)
                    .HasColumnName("ID");

                entity.Property(e => e.Theme).HasColumnType("text");
            });

            modelBuilder.Entity<UserThemeEntity>(entity =>
            {
                entity.ToTable("UserThemes");

                entity.Property(e => e.Id)
                    .HasMaxLength(8)
                    .HasColumnName("ID");

                entity.Property(e => e.Downloads).HasColumnType("int(11)");

                entity.Property(e => e.OwnerId)
                    .HasColumnType("text")
                    .HasColumnName("OwnerID");

                entity.Property(e => e.Version)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<WebhookEntity>(entity =>
            {
                entity.HasKey(e => new { e.ServerId, e.Name })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 32 });

                entity.ToTable("Webhooks");

                entity.Property(e => e.ServerId)
                    .HasColumnType("text")
                    .HasColumnName("ServerID");

                entity.Property(e => e.Name).HasColumnType("text");

                entity.Property(e => e.WebhookUrl)
                    .HasColumnType("text")
                    .HasColumnName("WebhookURL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
