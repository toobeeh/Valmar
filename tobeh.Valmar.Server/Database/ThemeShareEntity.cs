using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class ThemeShareEntity
    {
        [Key]
        [Column("ID")]
        [StringLength(8)]
        public string Id { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Theme { get; set; } = null!;
    }
}
