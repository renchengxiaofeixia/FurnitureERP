﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("erp_module")]
public partial class ErpModule
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string ModuleName { get; set; } = null!;

    [StringLength(50)]
    public string ModuleNo { get; set; } = null!;

    public int LoopNum { get; set; }

    [StringLength(10)]
    public string Prefix { get; set; } = null!;

    [StringLength(10)]
    public string Ending { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    public Guid MerchantGuid { get; set; }
}
