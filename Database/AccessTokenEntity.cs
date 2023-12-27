using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class AccessTokenEntity
    {
        public int Login { get; set; }
        public string AccessToken1 { get; set; } = null!;
        public DateOnly CreatedAt { get; set; }
    }
}
