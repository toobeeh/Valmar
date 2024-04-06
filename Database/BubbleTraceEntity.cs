using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class BubbleTraceEntity
    {
        [Column(TypeName = "text")]
        public string Date { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        [Column(TypeName = "int(11)")]
        public int Bubbles { get; set; }
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
    }
}
