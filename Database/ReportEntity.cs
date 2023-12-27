using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class ReportEntity
    {
        public string LobbyId { get; set; } = null!;
        public int ObserveToken { get; set; }
        public string Report1 { get; set; } = null!;
        public string Date { get; set; } = null!;
    }
}
