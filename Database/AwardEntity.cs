using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class AwardEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public sbyte Rarity { get; set; }
        public string Description { get; set; } = null!;
    }
}
