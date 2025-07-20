using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class JwtScopeEntity
    {
        /// <summary>
        /// The identifier of the scope that an application might access
        /// </summary>
        [Key]
        [StringLength(30)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the scope permissions
        /// </summary>
        [Column(TypeName = "text")]
        public string Description { get; set; } = null!;
    }
}