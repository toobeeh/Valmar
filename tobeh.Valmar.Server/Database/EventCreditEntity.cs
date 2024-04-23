using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class EventCreditEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        [Key]
        [Column("EventDropID", TypeName = "int(11)")]
        public int EventDropId { get; set; }
        [Column(TypeName = "int(11)")]
        public int Credit { get; set; }
    }
}
