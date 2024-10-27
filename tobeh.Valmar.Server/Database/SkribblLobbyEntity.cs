using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class SkribblLobbyEntity
    {
        [Key] [StringLength(16)] public string LobbyId { get; set; } = null!;
        [Column(TypeName = "text")] public string Description { get; set; } = null!;
        public bool WhitelistAllowedServers { get; set; }

        /// <summary>
        /// Comma-separated list of long
        /// </summary>
        [Column(TypeName = "text")]
        public string AllowedServers { get; set; } = null!;

        [Column(TypeName = "bigint(20)")] public long? LobbyOwnershipClaim { get; set; }

        /// <summary>
        /// Skribbl lobby report
        /// </summary>
        [Column(TypeName = "text")]
        public string SkribblDetails { get; set; } = null!;

        [Column(TypeName = "bigint(20)")] public long FirstSeen { get; set; }
        [Column(TypeName = "bigint(20)")] public long LastUpdated { get; set; }
    }
}