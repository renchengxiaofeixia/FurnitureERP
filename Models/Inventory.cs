using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("inventory")]
public partial class Inventory
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string ItemName { get; set; } = null!;

    [StringLength(200)]
    public string ItemNo { get; set; } = null!;

    [StringLength(200)]
    public string StdItemNo { get; set; } = null!;

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    [StringLength(50)]
    public string? WareName { get; set; }

    [StringLength(50)]
    public string? PurchaseNo { get; set; }

    [StringLength(50)]
    public string? StorageNo { get; set; }

    [StringLength(50)]
    public string? SuppName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StorageTime { get; set; }

    [StringLength(200)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [StringLength(50)]
    public string? StorageType { get; set; }

    [StringLength(50)]
    public string? Tid { get; set; }

    public Guid MerchantGuid { get; set; }
}
