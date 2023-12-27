using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Valmar.Database
{
    public partial class EventEntity
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; } = null!;
        public int DayLength { get; set; }
        public string Description { get; set; } = null!;
        public string ValidFrom { get; set; } = null!;
        public sbyte Progressive { get; set; }
    }
}
