using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class ServerWebhookEntity
    {
        [Key]
        [Column(TypeName = "bigint(20)")]
        public long GuildId { get; set; }

        [Key] [StringLength(50)] public string Name { get; set; } = null!;
        [Column(TypeName = "text")] public string Url { get; set; } = null!;
    }
}