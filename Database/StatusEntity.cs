using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class StatusEntity
    {
        public string SessionId { get; set; } = null!;
        public string Status1 { get; set; } = null!;
        public string Date { get; set; } = null!;
    }
}
