using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Models;

[Table("logis_point")]
public partial class LogisPoint
{
    [Key]
    public int Id { get; set; }

    public Guid Guid { get; set; }

    /// <summary>
    /// 物流名称
    /// </summary>
    [StringLength(20)]
    public string LogisName { get; set; } = null!;

    /// <summary>
    /// 省
    /// </summary>
    [StringLength(20)]
    public string? State { get; set; }

    /// <summary>
    /// 市
    /// </summary>
    [StringLength(20)]
    public string? City { get; set; }

    /// <summary>
    /// 区
    /// </summary>
    [StringLength(20)]
    public string? District { get; set; }

    /// <summary>
    /// 物流点名称
    /// </summary>
    [StringLength(200)]
    public string PointName { get; set; } = null!;

    /// <summary>
    /// 物流点地址
    /// </summary>
    [StringLength(500)]
    public string? PointAdress { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [StringLength(50)]
    public string? PointMobile { get; set; }

    /// <summary>
    /// 最低价
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal LowestPrice { get; set; }

    /// <summary>
    /// 干线价格
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal GanPrice { get; set; }

    /// <summary>
    /// 支线价格
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ZhiPrice { get; set; }

    /// <summary>
    /// 是否包邮
    /// </summary>
    public bool IsPost { get; set; }

    /// <summary>
    /// 预计到货天数
    /// </summary>
    public int EstTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(200)]
    public string? Remark { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsUsing { get; set; }

    [StringLength(20)]
    public string Creator { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 商户GUID
    /// </summary>
    public Guid MerchantGuid { get; set; }

    public byte[] TimeStamp { get; set; } = null!;
}
