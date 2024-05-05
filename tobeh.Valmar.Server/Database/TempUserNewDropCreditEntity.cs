using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    [Table("TempUserNewDropCredit")]
    public partial class TempUserNewDropCreditEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Login { get; set; }
        public double Credit { get; set; }
    }
}
