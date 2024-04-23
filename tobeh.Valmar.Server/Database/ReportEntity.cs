using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class ReportEntity
    {
        [Key]
        [Column("LobbyID", TypeName = "text")]
        public string LobbyId { get; set; } = null!;
        [Key]
        [Column(TypeName = "int(11)")]
        public int ObserveToken { get; set; }
        [Column("Report", TypeName = "text")]
        public string Report1 { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Date { get; set; } = null!;
    }
}
