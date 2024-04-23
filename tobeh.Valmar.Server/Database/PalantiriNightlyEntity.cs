using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    [Table("PalantiriNightly")]
    public partial class PalantiriNightlyEntity
    {
        [Key]
        [Column(TypeName = "text")]
        public string Token { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Palantir { get; set; } = null!;
    }
}
