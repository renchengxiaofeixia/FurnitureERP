using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("item")]
public partial class Item
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(500)]
    public string ItemName { get; set; } = null!;

    [StringLength(200)]
    public string ItemNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Volume { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    [StringLength(50)]
    public string? SuppName { get; set; }

    public int PurchaseDays { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PackageQty { get; set; }

    public bool IsCom { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [Required]
    public bool? IsUsing { get; set; }

    [StringLength(500)]
    public string? PicPath { get; set; }

    public int SafeQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string? SellerNick { get; set; }

    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
