using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class SkribblOnlinePlayerEntity
    {
        [Key] [Column(TypeName = "int(11)")] public int Login { get; set; }
        [Key] [StringLength(16)] public string LobbyId { get; set; } = null!;
        [Key] [Column(TypeName = "int(11)")] public int LobbyPlayerId { get; set; }
        [Column(TypeName = "bigint(20)")] public long Timestamp { get; set; }
        [Column(TypeName = "bigint(20)")] public long OwnershipClaim { get; set; }
    }
}