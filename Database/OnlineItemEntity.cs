using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class OnlineItemEntity
    {
        public string ItemType { get; set; } = null!;
        public int Slot { get; set; }
        public long ItemId { get; set; }
        public string LobbyKey { get; set; } = null!;
        public int LobbyPlayerId { get; set; }
        public int Date { get; set; }
    }
}
