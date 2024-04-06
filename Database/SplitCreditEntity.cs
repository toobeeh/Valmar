using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class SplitCreditEntity
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        [Column(TypeName = "int(11)")]
        public int Split { get; set; }
        [Column(TypeName = "text")]
        public string RewardDate { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Comment { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int ValueOverride { get; set; }
    }
}
