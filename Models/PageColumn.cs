﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("page_column")]
public partial class PageColumn
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string PageName { get; set; } = null!;

    public string? ColumnJson { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    public Guid MerchantGuid { get; set; }
}
