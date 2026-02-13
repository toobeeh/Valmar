using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    [Index(nameof(Invite), Name = "Invite", IsUnique = true)]
    public partial class LobbyBotOptionEntity
    {
        [Key]
        [Column(TypeName = "bigint(20)")]
        public long GuildId { get; set; }

        [Column(TypeName = "bigint(20)")] public long? ChannelId { get; set; }
        [Column(TypeName = "text")] public string Name { get; set; } = null!;
        [Column(TypeName = "int(11)")] public int Invite { get; set; }
        [Column(TypeName = "text")] public string Prefix { get; set; } = null!;
        [Column(TypeName = "text")] public string? BotName { get; set; }
        [Required] public bool? ShowInvite { get; set; }
        [Required] public bool? ProxyLinks { get; set; }
        [Column(TypeName = "text")] public string? AnnouncementsWebhook { get; set; }
    }
}