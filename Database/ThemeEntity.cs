using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class ThemeEntity
    {
        [Key]
        [Column(TypeName = "text")]
        public string Ticket { get; set; } = null!;
        [Column("Theme", TypeName = "text")]
        public string Theme1 { get; set; } = null!;
        [Column(TypeName = "text")]
        public string ThumbnailLanding { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? ThumbnailGame { get; set; }
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Description { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Author { get; set; } = null!;
    }
}
