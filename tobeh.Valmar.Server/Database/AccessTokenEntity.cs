using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class AccessTokenEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        [Column("AccessToken", TypeName = "text")]
        public string AccessToken1 { get; set; } = null!;
        public DateOnly CreatedAt { get; set; }
    }
}
