using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class MemberEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        [Column("Member", TypeName = "text")]
        public string Member1 { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int Bubbles { get; set; }
        [Column(TypeName = "text")]
        public string Sprites { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int Drops { get; set; }
        [Column(TypeName = "int(11)")]
        public int Flag { get; set; }
        [Column(TypeName = "text")]
        public string? Emoji { get; set; }
        [Column(TypeName = "text")]
        public string? Patronize { get; set; }
        [Column(TypeName = "text")]
        public string? Customcard { get; set; }
        [Column(TypeName = "text")]
        public string? Scenes { get; set; }
        [Column(TypeName = "text")]
        public string Streamcode { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? RainbowSprites { get; set; }
        [Column(TypeName = "bigint(20)")]
        public long? AwardPackOpened { get; set; }
    }
}
