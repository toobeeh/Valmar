using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    /// <summary>
    /// Registered OAuth2 clients
    /// </summary>
    [Table("OAuth2Clients")]
    public partial class Oauth2ClientEntity
    {
        /// <summary>
        /// Identifier of the client
        /// </summary>
        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the client
        /// </summary>
        [Column(TypeName = "text")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Comma-separated allowed redirect uris of the authorization flow
        /// </summary>
        [Column(TypeName = "text")]
        public string RedirectUris { get; set; } = null!;

        /// <summary>
        /// Expiry in s of issued access token
        /// </summary>
        [Column(TypeName = "bigint(20)")]
        public long TokenExpiry { get; set; }

        /// <summary>
        /// Comma-separated list of scopes which this client uses
        /// </summary>
        [Column(TypeName = "text")]
        public string Scopes { get; set; } = null!;

        /// <summary>
        /// Whether this client has been verified by typo
        /// </summary>
        public bool Verified { get; set; }

        /// <summary>
        /// Owner typo id of this client
        /// </summary>
        [Column(TypeName = "int(11)")]
        public int Owner { get; set; }

        /// <summary>
        /// The target audience that this client accesses
        /// </summary>
        [Column(TypeName = "text")]
        public string Audience { get; set; } = null!;
    }
}