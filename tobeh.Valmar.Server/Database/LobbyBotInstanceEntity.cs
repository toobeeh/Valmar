using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tobeh.Valmar.Server.Database
{
    public partial class LobbyBotInstanceEntity
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int Id { get; set; }
        [Column(TypeName = "text")]
        public string BotToken { get; set; } = null!;
        [Column(TypeName = "bigint(20)")]
        public long BotId { get; set; }
        [Column(TypeName = "text")]
        public string? ClaimedWorkerUlid { get; set; }
        [Column(TypeName = "text")]
        public string? ClaimUlid { get; set; }
    }
}
