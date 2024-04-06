using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class CloudTagEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Owner { get; set; }
        [Key]
        [Column("ImageID", TypeName = "bigint(20)")]
        public long ImageId { get; set; }
        [StringLength(30)]
        public string Title { get; set; } = null!;
        [StringLength(14)]
        public string Author { get; set; } = null!;
        public bool Own { get; set; }
        [Column(TypeName = "bigint(20)")]
        public long Date { get; set; }
        [StringLength(10)]
        public string Language { get; set; } = null!;
        public bool Private { get; set; }
    }
}
