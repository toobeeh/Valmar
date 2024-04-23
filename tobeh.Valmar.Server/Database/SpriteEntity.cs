using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class SpriteEntity
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;
        [Column("URL", TypeName = "text")]
        public string Url { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int Cost { get; set; }
        public bool Special { get; set; }
        [Column("EventDropID", TypeName = "int(11)")]
        public int EventDropId { get; set; }
        [Column(TypeName = "text")]
        public string? Artist { get; set; }
        [Column(TypeName = "int(11)")]
        public int Rainbow { get; set; }
    }
}
