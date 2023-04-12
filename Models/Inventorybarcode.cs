using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("inventorybarcode")]
public partial class Inventorybarcode
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string? BarCode { get; set; }

    [StringLength(100)]
    public string ItemName { get; set; } = null!;

    [StringLength(50)]
    public string ItemNo { get; set; } = null!;

    [StringLength(50)]
    public string StdItemNo { get; set; } = null!;

    [StringLength(50)]
    public string? PkNo { get; set; }

    [StringLength(50)]
    public string? PkName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PackageQuantity { get; set; }

    [StringLength(200)]
    public string? StorageNo { get; set; }

    [StringLength(50)]
    public string? WareName { get; set; }

    [StringLength(200)]
    public string? AreaName { get; set; }

    public Guid? AreaGuid { get; set; }

    [StringLength(50)]
    public string? SuppName { get; set; }

    public bool IsPrint { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PrintDate { get; set; }

    [StringLength(50)]
    public string? PrintUser { get; set; }

    public Guid? InvtGuid { get; set; }

    [StringLength(50)]
    public string? Tid { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }
}
