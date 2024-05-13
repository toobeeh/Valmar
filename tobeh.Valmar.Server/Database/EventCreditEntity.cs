using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class EventCreditEntity
    {
        [Key] [Column(TypeName = "int(11)")] public int Login { get; set; }

        [Key]
        [Column("EventDropID", TypeName = "int(11)")]
        public int EventDropId { get; set; }

        public float Credit { get; set; }
    }
}