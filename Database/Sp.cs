using System;
using System.Collections.Generic;

namespace Valmar.Database;

public partial class Sp
{
    public string LobbyKey { get; set; } = null!;

    public int LobbyPlayerId { get; set; }

    public int Sprite { get; set; }

    public string Date { get; set; } = null!;

    public int Slot { get; set; }

    public string Id { get; set; } = null!;

    public DateTime? DateAddCurrentTimestampInterval30Second { get; set; }
}
