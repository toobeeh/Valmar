using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class CardTemplateEntity
    {
        [Key]
        [StringLength(20)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Template { get; set; } = null!;
    }
}
