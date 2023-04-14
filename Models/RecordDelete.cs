using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Keyless]
[Table("record_delete")]
public partial class RecordDelete
{
    public long Id { get; set; }

    public long RecordId { get; set; }

    [StringLength(50)]
    public string JsonTypeName { get; set; } = null!;

    public string JsonRecord { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    public Guid MerchantGuid { get; set; }
}
