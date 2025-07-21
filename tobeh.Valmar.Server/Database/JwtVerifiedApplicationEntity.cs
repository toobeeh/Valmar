using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class JwtVerifiedApplicationEntity
    {
        /// <summary>
        /// Identifier of the application
        /// </summary>
        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the application
        /// </summary>
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Redirect uri of the authorization flow
        /// </summary>
        [Column(TypeName = "text")]
        public string RedirectUri { get; set; } = null!;

        /// <summary>
        /// Expiry in ms of an issued jwt
        /// </summary>
        [Column(TypeName = "bigint(20)")]
        public long JwtExpiry { get; set; }

        /// <summary>
        /// Comma-separated list of scopes which this application uses
        /// </summary>
        [Column(TypeName = "text")]
        public string JwtScopes { get; set; } = null!;
    }
}