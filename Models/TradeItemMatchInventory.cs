using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

/// <summary>
/// 主动分配库存
/// </summary>
[Table("trade_item_match_inventory")]
public partial class TradeItemMatchInventory
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string? Tid { get; set; }

    [StringLength(200)]
    public string? PicPath { get; set; }

    [StringLength(200)]
    public string? ItemName { get; set; }

    [StringLength(200)]
    public string? ItemNo { get; set; }

    [StringLength(200)]
    public string? StdItemNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal Num { get; set; }

    public long? NumIid { get; set; }

    [StringLength(50)]
    public string? OrderFrom { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Payment { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DivideOrderFee { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PartMjzDiscount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [StringLength(100)]
    public string? SellerNick { get; set; }

    public bool IsMade { get; set; }

    [StringLength(200)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(20)]
    public string Creator { get; set; } = null!;

    public Guid MerchantGuid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AllocInventoryDate { get; set; }

    [StringLength(254)]
    public string? AllocInventoryUser { get; set; }

    public long? InventoryId { get; set; }

    [StringLength(50)]
    public string? SuppName { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
