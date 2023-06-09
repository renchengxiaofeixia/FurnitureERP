﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("item_package")]
public partial class ItemPackage
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(100)]
    public string ItemNo { get; set; } = null!;

    [StringLength(100)]
    public string PackageNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    public Guid MerchantGuid { get; set; }
}
