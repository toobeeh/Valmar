using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    [Keyless]
    public partial class LobEntity
    {
        [Column("PlayerLobbyID", TypeName = "mediumtext")]
        public string? PlayerLobbyId { get; set; }
        [Column("JSON_UNQUOTE(DcName)")]
        public string? JsonUnquoteDcName { get; set; }
        [Column("Name_exp_3", TypeName = "mediumtext")]
        public string? NameExp3 { get; set; }
    }
}
