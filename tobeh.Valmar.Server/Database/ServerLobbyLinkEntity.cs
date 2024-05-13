using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class ServerLobbyLinkEntity
    {
        [Key]
        [Column(TypeName = "bigint(20)")]
        public long GuildId { get; set; }

        [Key] [Column(TypeName = "int(11)")] public int Login { get; set; }
        [Key] [StringLength(100)] public string Link { get; set; } = null!;
        public bool SlotAvailable { get; set; }
        [Key] [StringLength(20)] public string Username { get; set; } = null!;
        [Column(TypeName = "bigint(20)")] public long Timestamp { get; set; }
    }
}