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

    /// <summary>
    /// 店铺名称
    /// </summary>
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

    /// <summary>
    /// 平台扣点
    /// </summary>
    [Column(TypeName = "decimal(18, 3)")]
    public decimal Points { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [StringLength(50)]
    public string? Phone { get; set; }

    /// <summary>
    /// 店铺类型：淘宝，拼多多，抖音
    /// </summary>
    [StringLength(50)]
    public string? ShopType { get; set; }

    /// <summary>
    /// 京东店铺ID
    /// </summary>
    public long? VenderId { get; set; }

    public Guid MerchantGuid { get; set; }
}
