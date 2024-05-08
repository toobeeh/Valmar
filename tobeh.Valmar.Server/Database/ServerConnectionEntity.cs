using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class ServerConnectionEntity
    {
        [Key] [Column(TypeName = "int(11)")] public int Login { get; set; }

        [Key]
        [Column(TypeName = "bigint(20)")]
        public long GuildId { get; set; }

        public bool Ban { get; set; }
    }
}