using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    [Keyless]
    public partial class SpEntity
    {
        [Column(TypeName = "text")]
        public string LobbyKey { get; set; } = null!;
        [Column("LobbyPlayerID", TypeName = "int(11)")]
        public int LobbyPlayerId { get; set; }
        [Column(TypeName = "int(11)")]
        public int Sprite { get; set; }
        [Column(TypeName = "text")]
        public string Date { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int Slot { get; set; }
        [Column("ID", TypeName = "text")]
        public string Id { get; set; } = null!;
        [Column("DATE_ADD(CURRENT_TIMESTAMP, INTERVAL -30 SECOND)", TypeName = "datetime")]
        public DateTime? DateAddCurrentTimestampInterval30Second { get; set; }
    }
}
