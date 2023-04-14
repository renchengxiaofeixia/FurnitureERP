using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("package_imp")]
public partial class PackageImp
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(500)]
    public string PackageName { get; set; } = null!;

    [StringLength(200)]
    public string PackageNo { get; set; } = null!;

    [StringLength(50)]
    public string? LengthWidthHeight { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Volume { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    public int PurchaseDays { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    public int SafeQty { get; set; }

    public Guid MerchantGuid { get; set; }
}
