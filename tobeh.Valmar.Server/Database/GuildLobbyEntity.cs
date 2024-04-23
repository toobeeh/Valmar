using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class GuildLobbyEntity
    {
        [Key]
        [Column("GuildID", TypeName = "text")]
        public string GuildId { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Lobbies { get; set; } = null!;
    }
}
