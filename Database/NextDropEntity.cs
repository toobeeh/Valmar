using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    [Table("NextDrop")]
    public partial class NextDropEntity
    {
        [Key]
        [Column("DropID", TypeName = "bigint(20)")]
        public long DropId { get; set; }
        [Column(TypeName = "text")]
        public string CaughtLobbyKey { get; set; } = null!;
        [Column("CaughtLobbyPlayerID", TypeName = "text")]
        public string CaughtLobbyPlayerId { get; set; } = null!;
        [Column(TypeName = "text")]
        public string ValidFrom { get; set; } = null!;
        [Column("EventDropID", TypeName = "int(11)")]
        public int EventDropId { get; set; }
        [Column(TypeName = "int(11)")]
        public int LeagueWeight { get; set; }
    }
}
