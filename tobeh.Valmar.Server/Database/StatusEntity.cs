using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    [Table("Status")]
    public partial class StatusEntity
    {
        [Key]
        [Column("SessionID", TypeName = "text")]
        public string SessionId { get; set; } = null!;
        [Column("Status", TypeName = "text")]
        public string Status1 { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Date { get; set; } = null!;
    }
}
