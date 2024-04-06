using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class SpriteProfileEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        [Key]
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Combo { get; set; } = null!;
        [Column(TypeName = "text")]
        public string RainbowSprites { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Scene { get; set; } = null!;
    }
}
