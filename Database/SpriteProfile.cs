using System;
using System.Collections.Generic;

namespace Valmar.Database;

public partial class SpriteProfile
{
    public int Login { get; set; }

    public string Name { get; set; } = null!;

    public string Combo { get; set; } = null!;

    public string RainbowSprites { get; set; } = null!;

    public string Scene { get; set; } = null!;
}
