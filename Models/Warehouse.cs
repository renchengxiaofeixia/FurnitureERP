﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Keyless]
[Table("warehouse")]
public partial class Warehouse
{
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(100)]
    public string WarehouseName { get; set; } = null!;

    [StringLength(200)]
    public string Remark { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [Required]
    public bool? IsUsing { get; set; }

    public Guid MerchantGuid { get; set; }
}