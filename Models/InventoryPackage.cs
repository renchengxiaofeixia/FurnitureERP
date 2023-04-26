using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("inventory_package")]
public partial class InventoryPackage
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string PackageName { get; set; } = null!;

    [StringLength(200)]
    public string PackageNo { get; set; } = null!;

    [StringLength(200)]
    public string StdPackageNo { get; set; } = null!;

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    [StringLength(50)]
    public string WareName { get; set; } = null!;

    [StringLength(50)]
    public string AreaName { get; set; } = null!;

    [StringLength(50)]
    public string LocationName { get; set; } = null!;

    [StringLength(50)]
    public string? PurchaseNo { get; set; }

    [StringLength(50)]
    public string StorageNo { get; set; } = null!;

    [StringLength(50)]
    public string SuppName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime StorageTime { get; set; }

    [StringLength(200)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [StringLength(50)]
    public string StorageType { get; set; } = null!;

    [StringLength(50)]
    public string? Tid { get; set; }

    public Guid MerchantGuid { get; set; }
}
