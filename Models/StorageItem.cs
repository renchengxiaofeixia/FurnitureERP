using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("storage_item")]
public partial class StorageItem
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string? StorageNo { get; set; }

    [StringLength(200)]
    public string? ItemName { get; set; }

    [StringLength(200)]
    public string? ItemNo { get; set; }

    [StringLength(200)]
    public string? StdItemNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    public int PurchaseNum { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public int StorageNum { get; set; }

    [StringLength(50)]
    public string? PurchaseNo { get; set; }

    public bool IsMade { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    [StringLength(50)]
    public string? SuppName { get; set; }

    public Guid MerchantGuid { get; set; }
}
