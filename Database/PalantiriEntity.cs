using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    [Table("Palantiri")]
    public partial class PalantiriEntity
    {
        [Key]
        [Column(TypeName = "text")]
        public string Token { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Palantir { get; set; } = null!;
    }
}
