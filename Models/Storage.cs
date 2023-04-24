using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("storage")]
public partial class Storage
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 入库单号
    /// </summary>
    [StringLength(200)]
    public string? StorageNo { get; set; }

    /// <summary>
    /// 入库日期
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime StorageDate { get; set; }

    /// <summary>
    /// 入库类型：正常添加入库，采购单入库
    /// </summary>
    [StringLength(20)]
    public string? StorageType { get; set; }

    /// <summary>
    /// 供应商名
    /// </summary>
    [StringLength(50)]
    public string? SuppName { get; set; }

    /// <summary>
    /// 仓库名
    /// </summary>
    [StringLength(20)]
    public string? WareName { get; set; }

    /// <summary>
    /// 来源于采购单的销售单号
    /// </summary>
    [StringLength(50)]
    public string? Tid { get; set; }

    /// <summary>
    /// 总金额
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal AggregateAmount { get; set; }

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
    /// 采购单号
    /// </summary>
    [StringLength(50)]
    public string? PurchaseNo { get; set; }

    /// <summary>
    /// 支付金额
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PaidFee { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }
}
