using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("purchase")]
public partial class Purchase
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 采购单号
    /// </summary>
    [StringLength(50)]
    public string PurchaseNo { get; set; } = null!;

    /// <summary>
    /// 供应商名
    /// </summary>
    [StringLength(50)]
    public string SuppName { get; set; } = null!;

    /// <summary>
    /// 仓库名
    /// </summary>
    [StringLength(50)]
    public string WareName { get; set; } = null!;

    /// <summary>
    /// 总金额
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal AggregateAmount { get; set; }

    /// <summary>
    /// 采购时间
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? PurchaseOrderDate { get; set; }

    /// <summary>
    /// 结算方式
    /// </summary>
    [StringLength(50)]
    public string SettlementMode { get; set; } = null!;

    /// <summary>
    /// 是否审核
    /// </summary>
    public bool IsAudit { get; set; }

    /// <summary>
    /// 审核人
    /// </summary>
    [StringLength(50)]
    public string? AuditUser { get; set; }

    /// <summary>
    /// 审核日期
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? AuditDate { get; set; }

    /// <summary>
    /// 是否打印
    /// </summary>
    public bool IsPrint { get; set; }

    /// <summary>
    /// 打印人
    /// </summary>
    [StringLength(50)]
    public string? PrintUser { get; set; }

    /// <summary>
    /// 打印日期
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? PrintDate { get; set; }

    /// <summary>
    /// 外单号（工厂送货单号）
    /// </summary>
    [StringLength(50)]
    public string? OuterNo { get; set; }

    /// <summary>
    /// 采购下单时间
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime Created { get; set; }

    /// <summary>
    /// 交付日期
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime DeliveryDate { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    [StringLength(50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 付款金额
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PaidFee { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500)]
    public string? Remark { get; set; }

    /// <summary>
    /// 是否来源销售单
    /// </summary>
    public bool IsFromTrade { get; set; }

    /// <summary>
    /// 销售订单号
    /// </summary>
    [StringLength(50)]
    public string? Tid { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
