using System;
using System.Collections.Generic;

namespace Valmar.Database;

public partial class GuildSetting
{
    public string GuildId { get; set; } = null!;

    public string Settings { get; set; } = null!;
}
