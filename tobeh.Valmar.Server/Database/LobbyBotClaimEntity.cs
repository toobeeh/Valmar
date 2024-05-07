using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    [Index(nameof(InstanceId), Name = "UniqueInstance", IsUnique = true)]
    public partial class LobbyBotClaimEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        [Column(TypeName = "int(11)")]
        public int InstanceId { get; set; }
        [Column(TypeName = "bigint(20)")]
        public long ClaimTimestamp { get; set; }
        [Column(TypeName = "bigint(20)")]
        public long GuildId { get; set; }
    }
}
