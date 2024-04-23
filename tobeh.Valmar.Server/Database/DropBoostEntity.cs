using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class DropBoostEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        [Key]
        [Column("StartUTCS")]
        [StringLength(15)]
        public string StartUtcs { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int DurationS { get; set; }
        [Column(TypeName = "text")]
        public string Factor { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int CooldownBonusS { get; set; }
    }
}
