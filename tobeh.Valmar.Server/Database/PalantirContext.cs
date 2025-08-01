﻿using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class PalantirContext : DbContext
    {
        public virtual DbSet<AccessTokenEntity> AccessTokens { get; set; } = null!;
        public virtual DbSet<AwardEntity> Awards { get; set; } = null!;
        public virtual DbSet<AwardeeEntity> Awardees { get; set; } = null!;
        public virtual DbSet<BoostSplitEntity> BoostSplits { get; set; } = null!;
        public virtual DbSet<BubbleTraceEntity> BubbleTraces { get; set; } = null!;
        public virtual DbSet<CardTemplateEntity> CardTemplates { get; set; } = null!;
        public virtual DbSet<CloudTagEntity> CloudTags { get; set; } = null!;
        public virtual DbSet<CurrrentDropEntity> CurrrentDrops { get; set; } = null!;
        public virtual DbSet<DropBoostEntity> DropBoosts { get; set; } = null!;
        public virtual DbSet<EventEntity> Events { get; set; } = null!;
        public virtual DbSet<EventCreditEntity> EventCredits { get; set; } = null!;
        public virtual DbSet<EventDropEntity> EventDrops { get; set; } = null!;
        public virtual DbSet<GuildLobbyEntity> GuildLobbies { get; set; } = null!;
        public virtual DbSet<GuildSettingEntity> GuildSettings { get; set; } = null!;
        public virtual DbSet<LegacyDropCountEntity> LegacyDropCounts { get; set; } = null!;
        public virtual DbSet<LobbyBotClaimEntity> LobbyBotClaims { get; set; } = null!;
        public virtual DbSet<LobbyBotInstanceEntity> LobbyBotInstances { get; set; } = null!;
        public virtual DbSet<LobbyBotOptionEntity> LobbyBotOptions { get; set; } = null!;
        public virtual DbSet<MemberEntity> Members { get; set; } = null!;
        public virtual DbSet<Oauth2AuthorizationCodeEntity> Oauth2AuthorizationCodes { get; set; } = null!;
        public virtual DbSet<Oauth2ClientEntity> Oauth2Clients { get; set; } = null!;
        public virtual DbSet<Oauth2ScopeEntity> Oauth2Scopes { get; set; } = null!;
        public virtual DbSet<OnlineItemEntity> OnlineItems { get; set; } = null!;
        public virtual DbSet<PalantiriEntity> Palantiris { get; set; } = null!;
        public virtual DbSet<PalantiriNightlyEntity> PalantiriNightlies { get; set; } = null!;
        public virtual DbSet<PastDropEntity> PastDrops { get; set; } = null!;
        public virtual DbSet<SceneEntity> Scenes { get; set; } = null!;
        public virtual DbSet<SceneThemeEntity> SceneThemes { get; set; } = null!;
        public virtual DbSet<ServerConnectionEntity> ServerConnections { get; set; } = null!;
        public virtual DbSet<ServerWebhookEntity> ServerWebhooks { get; set; } = null!;
        public virtual DbSet<SkribblLobbyEntity> SkribblLobbies { get; set; } = null!;
        public virtual DbSet<SkribblOnlinePlayerEntity> SkribblOnlinePlayers { get; set; } = null!;
        public virtual DbSet<SplitCreditEntity> SplitCredits { get; set; } = null!;
        public virtual DbSet<SpriteEntity> Sprites { get; set; } = null!;
        public virtual DbSet<SpriteProfileEntity> SpriteProfiles { get; set; } = null!;
        public virtual DbSet<TemporaryPatronEntity> TemporaryPatrons { get; set; } = null!;
        public virtual DbSet<ThemeShareEntity> ThemeShares { get; set; } = null!;
        public virtual DbSet<TypoAnnouncementEntity> TypoAnnouncements { get; set; } = null!;
        public virtual DbSet<UserThemeEntity> UserThemes { get; set; } = null!;

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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("name=ConnectionStrings:Palantir", ServerVersion.Parse("11.8.2-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_uca1400_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<AccessTokenEntity>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.Property(e => e.Login).ValueGeneratedNever();
            });

            modelBuilder.Entity<BoostSplitEntity>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<BubbleTraceEntity>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<CardTemplateEntity>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<CloudTagEntity>(entity =>
            {
                entity.HasKey(e => new { e.Owner, e.ImageId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });

            modelBuilder.Entity<CurrrentDropEntity>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<DropBoostEntity>(entity =>
            {
                entity.HasKey(e => new { e.Login, e.StartUtcs })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });

            modelBuilder.Entity<EventEntity>(entity => { entity.Property(e => e.EventId).ValueGeneratedNever(); });

            modelBuilder.Entity<EventCreditEntity>(entity =>
            {
                entity.HasKey(e => new { e.Login, e.EventDropId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });

            modelBuilder.Entity<EventDropEntity>(entity =>
            {
                entity.Property(e => e.EventDropId).ValueGeneratedNever();
            });

            modelBuilder.Entity<GuildLobbyEntity>(entity =>
            {
                entity.HasKey(e => e.GuildId)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });
            });

            modelBuilder.Entity<GuildSettingEntity>(entity =>
            {
                entity.HasKey(e => e.GuildId)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });
            });

            modelBuilder.Entity<LegacyDropCountEntity>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.Property(e => e.Login).ValueGeneratedNever();
            });

            modelBuilder.Entity<LobbyBotClaimEntity>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.Property(e => e.Login).ValueGeneratedNever();
            });

            modelBuilder.Entity<LobbyBotOptionEntity>(entity =>
            {
                entity.HasKey(e => e.GuildId)
                    .HasName("PRIMARY");

                entity.Property(e => e.GuildId).ValueGeneratedNever();
            });

            modelBuilder.Entity<MemberEntity>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.Property(e => e.Login).ValueGeneratedNever();
            });

            modelBuilder.Entity<Oauth2AuthorizationCodeEntity>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Oauth2ClientEntity>(entity =>
            {
                entity.HasAnnotation("Relational:Comment", "Registered OAuth2 clients");
            });

            modelBuilder.Entity<Oauth2ScopeEntity>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PRIMARY");

                entity.HasAnnotation("Relational:Comment", "Scopes available in OAuth2 for JWT claims");
            });

            modelBuilder.Entity<OnlineItemEntity>(entity =>
            {
                entity.HasKey(e => new { e.ItemType, e.Slot, e.LobbyKey, e.LobbyPlayerId, e.Date })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 0, 32, 0, 0 });
            });

            modelBuilder.Entity<PalantiriEntity>(entity =>
            {
                entity.HasKey(e => e.Token)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });
            });

            modelBuilder.Entity<PalantiriNightlyEntity>(entity =>
            {
                entity.HasKey(e => e.Token)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });
            });

            modelBuilder.Entity<PastDropEntity>(entity =>
            {
                entity.HasKey(e => new { e.DropId, e.CaughtLobbyKey, e.CaughtLobbyPlayerId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
            });

            modelBuilder.Entity<SceneEntity>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<SceneThemeEntity>(entity =>
            {
                entity.HasKey(e => new { e.SceneId, e.Shift })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });

            modelBuilder.Entity<ServerConnectionEntity>(entity =>
            {
                entity.HasKey(e => new { e.Login, e.GuildId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });

            modelBuilder.Entity<ServerWebhookEntity>(entity =>
            {
                entity.HasKey(e => new { e.GuildId, e.Name })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });

            modelBuilder.Entity<SkribblLobbyEntity>(entity =>
            {
                entity.HasKey(e => e.LobbyId)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<SkribblOnlinePlayerEntity>(entity =>
            {
                entity.HasKey(e => new { e.Login, e.LobbyId, e.LobbyPlayerId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
            });

            modelBuilder.Entity<SpriteEntity>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<SpriteProfileEntity>(entity =>
            {
                entity.HasKey(e => new { e.Login, e.Name })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 32 });
            });

            modelBuilder.Entity<TemporaryPatronEntity>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PRIMARY");

                entity.Property(e => e.Login).ValueGeneratedNever();
            });

            modelBuilder.Entity<TypoAnnouncementEntity>(entity =>
            {
                entity.HasKey(e => e.Date)
                    .HasName("PRIMARY");

                entity.Property(e => e.Date).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}