using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    [Table("CurrrentDrop")]
    public partial class CurrrentDropEntity
    {
        /// <summary>
        /// The ID of the drop
        /// </summary>
        [Key]
        [Column(TypeName = "bigint(20)")]
        public long Id { get; set; }

        /// <summary>
        /// The timestamp ms where claims start to be accepted
        /// </summary>
        [Column(TypeName = "bigint(20)")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Whether anyone has claimed the drop
        /// </summary>
        public bool Claimed { get; set; }

        /// <summary>
        /// Whether the drop has been cleared and accepts no more claims
        /// </summary>
        public bool Cleared { get; set; }

        /// <summary>
        /// The optional id of an event drop as which this drop appears
        /// </summary>
        [Column(TypeName = "int(11)")]
        public int? EventDropId { get; set; }
    }
}