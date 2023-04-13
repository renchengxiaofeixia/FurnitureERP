using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("inventory_item_adjust")]
public partial class InventoryItemAdjust
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string? AdjustNo { get; set; }

    [StringLength(100)]
    public string ItemName { get; set; } = null!;

    [StringLength(100)]
    public string ItemNo { get; set; } = null!;

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [StringLength(50)]
    public string? WareName { get; set; }

    [StringLength(50)]
    public string? PurchaseNo { get; set; }

    [StringLength(50)]
    public string? SuppName { get; set; }

    public int AdjustQuantity { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
