﻿using System;
using System.Collections.Generic;

namespace Valmar.Database;

public partial class EventDrop
{
    public int EventDropId { get; set; }

    public int EventId { get; set; }

    public string Url { get; set; } = null!;

    public string Name { get; set; } = null!;
}
