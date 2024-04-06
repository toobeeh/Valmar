using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class LobbyEntity
    {
        [Key]
        [Column("LobbyID", TypeName = "text")]
        public string LobbyId { get; set; } = null!;
        [Column("Lobby", TypeName = "text")]
        public string Lobby1 { get; set; } = null!;
    }
}
