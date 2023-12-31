﻿using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class ThemeEntity
    {
        public string Ticket { get; set; } = null!;
        public string Theme1 { get; set; } = null!;
        public string ThumbnailLanding { get; set; } = null!;
        public string? ThumbnailGame { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Author { get; set; } = null!;
    }
}
