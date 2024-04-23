using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class UserThemeEntity
    {
        [Key]
        [Column("ID")]
        [StringLength(8)]
        public string Id { get; set; } = null!;
        [Column("OwnerID", TypeName = "text")]
        public string OwnerId { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int Version { get; set; }
        [Column(TypeName = "int(11)")]
        public int Downloads { get; set; }
    }
}
