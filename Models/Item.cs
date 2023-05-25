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

    /// <summary>
    /// 商品名称
    /// </summary>
    [StringLength(500)]
    public string ItemName { get; set; } = null!;

    /// <summary>
    /// 商品编码
    /// </summary>
    [StringLength(200)]
    public string ItemNo { get; set; } = null!;

    /// <summary>
    /// 体积
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Volume { get; set; }

    /// <summary>
    /// 采购价
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    /// <summary>
    /// 供应商名
    /// </summary>
    [StringLength(50)]
    public string? SuppName { get; set; }

    /// <summary>
    /// 采购周期
    /// </summary>
    public int PurchaseDays { get; set; }

    /// <summary>
    /// 包装件数
    /// </summary>
    public int PackageQty { get; set; }

    /// <summary>
    /// 是否组合商品
    /// </summary>
    public bool IsCom { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [Required]
    public bool? IsUsing { get; set; }

    /// <summary>
    /// 商品图
    /// </summary>
    [StringLength(500)]
    public string? PicPath { get; set; }

    /// <summary>
    /// 安全库存
    /// </summary>
    public int SafeQty { get; set; }

    /// <summary>
    /// 销售价
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// 所属店铺
    /// </summary>
    [StringLength(500)]
    public string? SellerNick { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;

    /// <summary>
    /// 风格
    /// </summary>
    [StringLength(100)]
    public string Style { get; set; } = null!;

    /// <summary>
    /// 品类
    /// </summary>
    [StringLength(100)]
    public string Class { get; set; } = null!;

    /// <summary>
    /// 品牌
    /// </summary>
    [StringLength(100)]
    public string Brand { get; set; } = null!;

    /// <summary>
    /// 空间
    /// </summary>
    [StringLength(100)]
    public string Space { get; set; } = null!;

    /// <summary>
    /// 自定义分类
    /// </summary>
    [StringLength(255)]
    [Unicode(false)]
    public string Cate { get; set; } = null!;
}
