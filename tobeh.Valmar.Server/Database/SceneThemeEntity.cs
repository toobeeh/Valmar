using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tobeh.Valmar.Server.Database
{
    public partial class SceneThemeEntity
    {
        [Column(TypeName = "text")] public string Name { get; set; } = null!;
        [Key] [Column(TypeName = "int(11)")] public int SceneId { get; set; }
        [Key] [Column(TypeName = "int(11)")] public int Shift { get; set; }
    }
}