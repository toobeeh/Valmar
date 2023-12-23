using System;
using System.Collections.Generic;

namespace Valmar.Database;

public partial class Webhook
{
    public string ServerId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string WebhookUrl { get; set; } = null!;
}
