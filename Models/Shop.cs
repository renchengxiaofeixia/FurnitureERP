using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("shop")]
public partial class Shop
{
    [Key]
    public long Id { get; set; }

    public Guid Guid { get; set; }

    [StringLength(200)]
    public string SellerNick { get; set; } = null!;

    [StringLength(1000)]
    public string? AppKey { get; set; }

    [StringLength(1000)]
    public string? AppSercert { get; set; }

    [StringLength(1000)]
    public string? SessionKey { get; set; }

    public bool IsUsing { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(20)]
    public string? Creator { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal Points { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? ShopType { get; set; }

    public long? VenderId { get; set; }

    public Guid MerchantGuid { get; set; }
}
