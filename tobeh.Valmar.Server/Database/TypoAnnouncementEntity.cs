using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class TypoAnnouncementEntity
    {
        [Key]
        [Column(TypeName = "bigint(20)")]
        public long Date { get; set; }

        [Column(TypeName = "enum('changelog','announcement')")]
        public string Type { get; set; } = null!;

        [Column(TypeName = "text")] public string? AffectedTypoVersion { get; set; }
        [Column(TypeName = "text")] public string Title { get; set; } = null!;
        [Column(TypeName = "text")] public string Content { get; set; } = null!;
    }
}