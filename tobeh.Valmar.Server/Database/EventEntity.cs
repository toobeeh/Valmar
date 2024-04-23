using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class EventEntity
    {
        [Key]
        [Column("EventID", TypeName = "int(11)")]
        public int EventId { get; set; }
        [Column(TypeName = "text")]
        public string EventName { get; set; } = null!;
        [Column(TypeName = "int(11)")]
        public int DayLength { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; } = null!;
        [Column(TypeName = "text")]
        public string ValidFrom { get; set; } = null!;
        [Column(TypeName = "tinyint(4)")]
        public sbyte Progressive { get; set; }
    }
}
