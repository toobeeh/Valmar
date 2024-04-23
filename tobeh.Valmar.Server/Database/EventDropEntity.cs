using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class EventDropEntity
    {
        [Key]
        [Column("EventDropID", TypeName = "int(11)")]
        public int EventDropId { get; set; }
        [Column("EventID", TypeName = "int(11)")]
        public int EventId { get; set; }
        [Column("URL", TypeName = "text")]
        public string Url { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;
    }
}
