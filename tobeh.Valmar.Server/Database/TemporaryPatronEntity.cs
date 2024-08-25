using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class TemporaryPatronEntity
    {
        [Key]
        [Column(TypeName = "bigint(20)")]
        public long Login { get; set; }
    }
}