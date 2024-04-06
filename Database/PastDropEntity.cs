using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class PastDropEntity
    {
        [Key]
        [Column("DropID", TypeName = "bigint(11)")]
        public long DropId { get; set; }
        [Key]
        [StringLength(50)]
        public string CaughtLobbyKey { get; set; } = null!;
        [Key]
        [Column("CaughtLobbyPlayerID")]
        [StringLength(20)]
        public string CaughtLobbyPlayerId { get; set; } = null!;
        [Column(TypeName = "text")]
        public string ValidFrom { get; set; } = null!;
        [Column("EventDropID", TypeName = "int(11)")]
        public int EventDropId { get; set; }
        [Column(TypeName = "int(11)")]
        public int LeagueWeight { get; set; }
    }
}
