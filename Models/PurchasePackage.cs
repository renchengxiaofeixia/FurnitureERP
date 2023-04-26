using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("purchase_package")]
public partial class PurchasePackage
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 采购单号
    /// </summary>
    [StringLength(50)]
    public string PurchaseNo { get; set; } = null!;

    public Guid PurchaseItemGuid { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    [StringLength(50)]
    public string SuppName { get; set; } = null!;

    /// <summary>
    /// 包件名称
    /// </summary>
    [StringLength(200)]
    public string PackageName { get; set; } = null!;

    /// <summary>
    /// 包件编码（可为定制编码）
    /// </summary>
    [StringLength(200)]
    public string PackageNo { get; set; } = null!;

    /// <summary>
    /// 标准包件编码
    /// </summary>
    [StringLength(200)]
    public string? StdPackageNo { get; set; }

    /// <summary>
    /// 采购价
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    /// <summary>
    /// 采购数量
    /// </summary>
    public int PurchaseNum { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// 入库数量
    /// </summary>
    public int StorageNum { get; set; }

    /// <summary>
    /// 取消数量
    /// </summary>
    public int CancelNum { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500)]
    public string? Remark { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateTime { get; set; }

    [StringLength(50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 交付日期
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// 是否定制
    /// </summary>
    public bool IsMade { get; set; }

    /// <summary>
    /// 订单商品GUID
    /// </summary>
    public Guid? OrderGuid { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }
}
