using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class GuildSettingEntity
    {
        [Key]
        [Column("GuildID", TypeName = "text")]
        public string GuildId { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Settings { get; set; } = null!;
    }
}
