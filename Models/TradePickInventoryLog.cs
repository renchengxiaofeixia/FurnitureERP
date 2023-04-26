using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

/// <summary>
/// 打印减库存
/// </summary>
[Table("trade_pick_inventory_log")]
public partial class TradePickInventoryLog
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public Guid TradeItemGuid { get; set; }

    [StringLength(200)]
    public string ItemNo { get; set; } = null!;

    [StringLength(50)]
    public string WareName { get; set; } = null!;

    [StringLength(50)]
    public string AreaName { get; set; } = null!;

    [StringLength(50)]
    public string LocationName { get; set; } = null!;

    public Guid InventoryGuid { get; set; }

    public long InventoryId { get; set; }

    [StringLength(50)]
    public string SuppName { get; set; } = null!;

    [StringLength(50)]
    public string? PurchaseNo { get; set; }

    [StringLength(50)]
    public string StorageNo { get; set; } = null!;

    public int InventoryQuantity { get; set; }

    public int MinusQuantity { get; set; }

    [StringLength(50)]
    public string Tid { get; set; } = null!;

    public Guid PrintSession { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    [StringLength(200)]
    public string? Remark { get; set; }
}
