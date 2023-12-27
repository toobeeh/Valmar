using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class UserThemeEntity
    {
        public string Id { get; set; } = null!;
        public string OwnerId { get; set; } = null!;
        public int Version { get; set; }
        public int Downloads { get; set; }
    }
}
