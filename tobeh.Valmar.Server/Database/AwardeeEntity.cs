using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class AwardeeEntity
    {
        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "smallint(6)")]
        public short Award { get; set; }
        [Column(TypeName = "int(6)")]
        public int OwnerLogin { get; set; }
        [Column(TypeName = "int(6)")]
        public int? AwardeeLogin { get; set; }
        [Column(TypeName = "bigint(20)")]
        public long? Date { get; set; }
        [Column("ImageID", TypeName = "bigint(20)")]
        public long? ImageId { get; set; }
    }
}
