using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    [Table("OAuth2AuthorizationCodes")]
    public partial class Oauth2AuthorizationCodeEntity
    {
        /// <summary>
        /// ULID acting as authorization code
        /// </summary>
        [Key]
        [StringLength(30)]
        public string Code { get; set; } = null!;

        /// <summary>
        /// The client id which this code was issued for
        /// </summary>
        [Column(TypeName = "int(11)")]
        public int ClientId { get; set; }

        /// <summary>
        /// The typo user which this code was issued for
        /// </summary>
        [Column(TypeName = "int(11)")]
        public int TypoId { get; set; }

        /// <summary>
        /// Unix timestamp where the code expires
        /// </summary>
        [Column(TypeName = "bigint(11)")]
        public long Expiry { get; set; }
    }
}