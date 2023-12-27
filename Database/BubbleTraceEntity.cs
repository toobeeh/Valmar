using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class BubbleTraceEntity
    {
        public string Date { get; set; } = null!;
        public int Login { get; set; }
        public int Bubbles { get; set; }
        public int Id { get; set; }
    }
}
