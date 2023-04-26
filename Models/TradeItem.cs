using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("trade_item")]
public partial class TradeItem
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(50)]
    public string Tid { get; set; } = null!;

    [StringLength(200)]
    public string? PicPath { get; set; }

    [StringLength(200)]
    public string ItemName { get; set; } = null!;

    [StringLength(200)]
    public string ItemNo { get; set; } = null!;

    [StringLength(200)]
    public string StdItemNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public int Num { get; set; }

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

    public bool IsCom { get; set; }

    [StringLength(200)]
    public string? Remark { get; set; }

    [StringLength(50)]
    public string? WareName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(20)]
    public string Creator { get; set; } = null!;

    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
