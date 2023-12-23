using System;
using System.Collections.Generic;

namespace Valmar.Database;

public partial class CloudTag
{
    public int Owner { get; set; }

    public long ImageId { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public bool Own { get; set; }

    public long Date { get; set; }

    public string Language { get; set; } = null!;

    public bool Private { get; set; }
}
