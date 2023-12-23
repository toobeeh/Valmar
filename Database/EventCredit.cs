using System;
using System.Collections.Generic;

namespace Valmar.Database;

public partial class EventCredit
{
    public int Login { get; set; }

    public int EventDropId { get; set; }

    public int Credit { get; set; }
}
