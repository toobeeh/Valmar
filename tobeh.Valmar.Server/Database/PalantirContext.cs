using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<DropBoostEntity> DropBoosts { get; set; } = null!;
        public virtual DbSet<EventEntity> Events { get; set; } = null!;
        public virtual DbSet<EventCreditEntity> EventCredits { get; set; } = null!;
        public virtual DbSet<EventDropEntity> EventDrops { get; set; } = null!;
        public virtual DbSet<GuildLobbyEntity> GuildLobbies { get; set; } = null!;
        public virtual DbSet<GuildSettingEntity> GuildSettings { get; set; } = null!;
        public virtual DbSet<LegacyDropCountEntity> LegacyDropCounts { get; set; } = null!;
        public virtual DbSet<LobEntity> Lobs { get; set; } = null!;
        public virtual DbSet<LobbyEntity> Lobbies { get; set; } = null!;
        public virtual DbSet<LobbyBotClaimEntity> LobbyBotClaims { get; set; } = null!;
        public virtual DbSet<LobbyBotInstanceEntity> LobbyBotInstances { get; set; } = null!;
        public virtual DbSet<LobbyBotOptionEntity> LobbyBotOptions { get; set; } = null!;
        public virtual DbSet<MemberEntity> Members { get; set; } = null!;
        public virtual DbSet<NextDropEntity> NextDrops { get; set; } = null!;
        public virtual DbSet<OnlineItemEntity> OnlineItems { get; set; } = null!;
        public virtual DbSet<OnlineSpriteEntity> OnlineSprites { get; set; } = null!;
        public virtual DbSet<PalantiriEntity> Palantiris { get; set; } = null!;
        public virtual DbSet<PalantiriNightlyEntity> PalantiriNightlies { get; set; } = null!;
        public virtual DbSet<PastDropEntity> PastDrops { get; set; } = null!;
        public virtual DbSet<ReportEntity> Reports { get; set; } = null!;
        public virtual DbSet<SceneEntity> Scenes { get; set; } = null!;
        public virtual DbSet<ServerConnectionEntity> ServerConnections { get; set; } = null!;
        public virtual DbSet<ServerWebhookEntity> ServerWebhooks { get; set; } = null!;
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("name=ConnectionStrings:Palantir",
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.3.2-mariadb"));
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

            modelBuilder.Entity<LobEntity>(entity => { entity.ToView("lobs"); });

            modelBuilder.Entity<LobbyEntity>(entity =>
            {
                entity.HasKey(e => e.LobbyId)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });
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

            modelBuilder.Entity<NextDropEntity>(entity =>
            {
                entity.HasKey(e => e.DropId)
                    .HasName("PRIMARY");

                entity.Property(e => e.DropId).ValueGeneratedNever();
            });

            modelBuilder.Entity<OnlineItemEntity>(entity =>
            {
                entity.HasKey(e => new { e.ItemType, e.Slot, e.LobbyKey, e.LobbyPlayerId, e.Date })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 0, 32, 0, 0 });
            });

            modelBuilder.Entity<OnlineSpriteEntity>(entity =>
            {
                entity.HasKey(e => new { e.LobbyKey, e.LobbyPlayerId, e.Slot })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 0, 0 });
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

            modelBuilder.Entity<ReportEntity>(entity =>
            {
                entity.HasKey(e => new { e.LobbyId, e.ObserveToken })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 0 });
            });

            modelBuilder.Entity<SceneEntity>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

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

            modelBuilder.Entity<SpEntity>(entity => { entity.ToView("sps"); });

            modelBuilder.Entity<SpriteEntity>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<SpriteProfileEntity>(entity =>
            {
                entity.HasKey(e => new { e.Login, e.Name })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 32 });
            });

            modelBuilder.Entity<StatusEntity>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });
            });

            modelBuilder.Entity<ThemeEntity>(entity =>
            {
                entity.HasKey(e => e.Ticket)
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32 });
            });

            modelBuilder.Entity<WebhookEntity>(entity =>
            {
                entity.HasKey(e => new { e.ServerId, e.Name })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 32, 32 });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}