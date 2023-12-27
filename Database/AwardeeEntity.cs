using System;
using System.Collections.Generic;

namespace Valmar.Database
{
    public partial class AwardeeEntity
    {
        public int Id { get; set; }
        public short Award { get; set; }
        public int OwnerLogin { get; set; }
        public int? AwardeeLogin { get; set; }
        public long? Date { get; set; }
        public long? ImageId { get; set; }
    }
}
