using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class TemporaryPatronEntity
    {
        [Key] [Column(TypeName = "int(20)")] public int Login { get; set; }
    }
}