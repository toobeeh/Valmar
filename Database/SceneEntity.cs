﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class SceneEntity
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Artist { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Color { get; set; } = null!;
        [Column("URL", TypeName = "text")]
        public string Url { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? GuessedColor { get; set; }
        [Column("EventID", TypeName = "int(11)")]
        public int EventId { get; set; }
        public bool Exclusive { get; set; }
    }
}
