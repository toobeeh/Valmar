using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class OnlineSpriteEntity
    {
        [Key]
        [Column(TypeName = "text")]
        public string LobbyKey { get; set; } = null!;
        [Key]
        [Column("LobbyPlayerID", TypeName = "int(11)")]
        public int LobbyPlayerId { get; set; }
        [Column(TypeName = "int(11)")]
        public int Sprite { get; set; }
        [Column(TypeName = "text")]
        public string Date { get; set; } = null!;
        [Key]
        [Column(TypeName = "int(11)")]
        public int Slot { get; set; }
        [Column("ID", TypeName = "text")]
        public string Id { get; set; } = null!;
    }
}
