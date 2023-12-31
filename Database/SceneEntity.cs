﻿using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class SceneEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Artist { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? GuessedColor { get; set; }
        public int EventId { get; set; }
        public bool Exclusive { get; set; }
    }
}
