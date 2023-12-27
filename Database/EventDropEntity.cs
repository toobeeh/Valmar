using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Valmar.Database
{
    public partial class EventDropEntity
    {
        [Key]
        public int EventDropId { get; set; }
        public int EventId { get; set; }
        public string Url { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
