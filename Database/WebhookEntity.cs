using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Valmar.Database
{
    public partial class WebhookEntity
    {
        [Key]
        [Column("ServerID", TypeName = "text")]
        public string ServerId { get; set; } = null!;
        [Key]
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;
        [Column("WebhookURL", TypeName = "text")]
        public string WebhookUrl { get; set; } = null!;
    }
}
