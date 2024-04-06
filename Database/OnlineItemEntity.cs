using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class OnlineItemEntity
    {
        [Key]
        [Column(TypeName = "text")]
        public string ItemType { get; set; } = null!;
        [Key]
        [Column(TypeName = "int(11)")]
        public int Slot { get; set; }
        [Column("ItemID", TypeName = "bigint(11)")]
        public long ItemId { get; set; }
        [Key]
        [Column(TypeName = "text")]
        public string LobbyKey { get; set; } = null!;
        [Key]
        [Column("LobbyPlayerID", TypeName = "int(11)")]
        public int LobbyPlayerId { get; set; }
        [Key]
        [Column(TypeName = "int(20)")]
        public int Date { get; set; }
    }
}
