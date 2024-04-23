using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class BoostSplitEntity
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Description { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Date { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int Value { get; set; }
    }
}
